using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;
using System.Diagnostics;

//EMGU
using Emgu.CV;
using Emgu.CV.Structure;

//SkinDetector
using HandGestureRecognition.SkinDetector;

//AForge Video
using AForge.Video;
using AForge.Video.DirectShow;

namespace HandGestureRecognition
{
    public partial class fHandGestureRecognition : Form
    {
        //Localization
        string lang = "it"; // en = English, it = Italiano, es = Español

        //Send Key Strokes
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        //Applications
        short selectedApplicationIndex = 0;
        List<string> applications = new List<string>();
        readonly Process thisApp = Process.GetCurrentProcess();
        Process selectedApp;

        //Camera Capture
        VideoCaptureDevice capture;
        List<string> connecteCameras;
        short camIndex = 0;
        short cameraMode = 0; //0 = Normal, 1 = Skin

        Image<Bgr, Byte> currentFrame;
        Bitmap currentBitmapFrame;
        short frameWidth = 640;
        short frameHeight = 480;

        //Fps
        Stopwatch stopwatch = new Stopwatch();
        short currentAvgFPS = 0;
        int waitTime = (1000 / 25);

        const short MAXFPSLIMIT = 60;

        //Hand Movement
        PointF previousHandPosition = new PointF(320, 480);
        const short CHANGEPERC = 9;
        short previousPerch = 100;

        //SkinDetector
        IColorSkinDetector skinDetector;
        AdaptiveSkinDetector detector;

        Hsv hsv_min;
        Hsv hsv_max;
        Ycc YCrCb_min;
        Ycc YCrCb_max;

        Seq<Point> hull;
        Seq<Point> filteredHull;
        Seq<MCvConvexityDefect> defects;
        MCvConvexityDefect[] defectArray;
        Rectangle handRect;
        MCvBox2D box;
        Ellipse ellip;


        public fHandGestureRecognition()
        {
            InitializeComponent();
        }

        private void fHandGestureReconition_Load(object sender, EventArgs e)
        {
            lErrorLog.Text = "";

            detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.NONE);

            hsv_min = new Hsv(0, 45, 0);
            hsv_max = new Hsv(20, 255, 255);
            YCrCb_min = new Ycc(0, 131, 80);
            YCrCb_max = new Ycc(255, 185, 135);
            box = new MCvBox2D();
            ellip = new Ellipse();

            skinDetector = new YCrCbSkinDetector();

            ChangeLocalization();
            UpdateConnectedCameras();
            UpdateRunningApplications();
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            stopwatch.Start();
            stopwatch.Reset();

            currentBitmapFrame = (Bitmap)eventArgs.Frame.Clone();
            currentBitmapFrame.RotateFlip(RotateFlipType.RotateNoneFlipX);
            currentFrame = new Image<Bgr, Byte>(currentBitmapFrame);

            frameWidth = (short)currentBitmapFrame.Width;
            frameHeight = (short)currentBitmapFrame.Height;

            Image<Gray, Byte> skin = skinDetector.DetectSkin(new Image<Bgr, Byte>(currentBitmapFrame), YCrCb_min, YCrCb_max);

            ExtractContourAndHull(skin);

            try
            {
                if (cameraMode == 0)
                    pbCurrentFrame.Image = currentFrame.Bitmap;
                else if (cameraMode == 1)
                    pbCurrentFrame.Image = skin.Bitmap;
            }
            catch { }

            stopwatch.Stop();

            waitTime = (int)stopwatch.ElapsedMilliseconds;
            if (waitTime < 1)
                waitTime = 1;
            if (1000 / waitTime > MAXFPSLIMIT)
                waitTime = 1000 / MAXFPSLIMIT;

            currentAvgFPS = (short)((currentAvgFPS + (1000 / waitTime)) / 2);

            //Console.WriteLine(currentAvgFPS + "fps " + waitTime + "milliseconds"); //Console debug
            miFps.Text = "Fps: " + currentAvgFPS;

            Thread.Sleep(waitTime);
        }

        // Help Methods
        public static List<string> GetAllConnectedCameras()
        {
            var cameraNames = new List<string>();

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
            {
                foreach (var device in searcher.Get())
                {
                    cameraNames.Add(device["Caption"].ToString());
                }
            }

            cameraNames.Sort();

            return cameraNames;
        }

        public void UpdateConnectedCameras(Boolean reset = false)
        {
            if (reset)
            {
                miCamera.DropDownItems.Clear();
                connecteCameras = null;
            }

            connecteCameras = GetAllConnectedCameras();

            short i = 0;
            foreach (string device in connecteCameras)
            {
                miCamera.DropDownItems.Add(i + ": " + device);
                miCamera.DropDownItems[i].Click += new System.EventHandler(SetCamIndex);
                i++;
            }
        }

        public Boolean isApplicationsListChanged()
        {
            if (Process.GetProcesses() == null)
                return false;

            string applicationName = "";
            short i = 0;
            foreach (var p in Process.GetProcesses())
            {
                if (!string.IsNullOrEmpty(p.MainWindowTitle))
                {
                    applicationName = p.MainWindowTitle;

                    //if (applicationName.Contains(" - ")) //Accorcia il titolo dell'applicazione 
                    //    applicationName = applicationName.Substring(applicationName.LastIndexOf('-') + 1).Trim();

                    if (!(applicationName.Equals("Microsoft Text Input Application") || applicationName.Equals("Hand Gesture Recognition")))
                    {
                        if (applications.Count > i)
                        {
                            if (!applications[i].Equals(applicationName))
                                return true;

                            i++;
                        }
                        else
                            return true;
                    }
                }
            }

            return false;
        }

        public void UpdateRunningApplications(Boolean reset = false)
        {
            if (Process.GetProcesses() == null)
                return;

            List<string> intermediate_list = new List<string>();
            string applicationName = "";
            short i = 0;
            if (reset)
            {
                miApplication.DropDownItems.Clear();
                applications.Clear();
            }

            foreach (var p in Process.GetProcesses())
            {
                if (!string.IsNullOrEmpty(p.MainWindowTitle))
                {
                    applicationName = p.MainWindowTitle;

                    //Console.WriteLine(i + ": " + applicationName); //Console debug

                    //if (applicationName.Contains(" - ")) //Accorcia il titolo dell'applicazione 
                    //    applicationName = applicationName.Substring(applicationName.LastIndexOf('-') + 1).Trim();

                    if (!(applicationName.Equals("Microsoft Text Input Application") || applicationName.Equals("Hand Gesture Recognition")))
                        intermediate_list.Add(applicationName);

                    i++;
                }
            }

            i = 0;
            foreach (string app in intermediate_list)
            {
                miApplication.DropDownItems.Add(app);
                miApplication.DropDownItems[i].Click += new System.EventHandler(SetApplicationSelected);

                applications.Add(app);
                i++;
            }

            if (!tCheckApplications.Enabled)
                tCheckApplications.Enabled = true;
        }

        public void SetApplicationSelected(object sender, System.EventArgs e)
        {
            short i = 0;
            foreach (string app in applications)
            {
                if (app.Equals(sender.ToString()))
                {
                    selectedApplicationIndex = i;
                    break;
                }
                i++;
            }

            foreach (var p in Process.GetProcesses())
            {
                if (p.MainWindowTitle.Contains(applications[selectedApplicationIndex]))
                    selectedApp = p;
            }
        }

        public void ChangeLocalization()
        {
            if (lang.Equals("en"))
            {
                miFile.Text = "File";
                miStart.Text = "Start";
                miPause.Text = "Pause";

                miApplication.Text = "Application";
                miSettings.Text = "Settings";
                miLanguage.Text = "Language";
                miEnglish.Text = "English";
                miItalian.Text = "Italian";
                miSpanish.Text = "Spanish";
                miCamera.Text = "Camera";
                miCameraMode.Text = "Camera Mode";
                miNormal.Text = "Normal";
                miSkin.Text = "Skin";

                miGuide.Text = "Guide";
            }
            else if (lang.Equals("it"))
            {
                miFile.Text = "File";
                miStart.Text = "Avvia";
                miPause.Text = "Pausa";

                miApplication.Text = "Applicazione";
                miSettings.Text = "Impostazioni";
                miLanguage.Text = "Lingua";
                miEnglish.Text = "Inglese";
                miItalian.Text = "Italiano";
                miSpanish.Text = "Spagnolo";
                miCamera.Text = "Camera";
                miCameraMode.Text = "Modalità Camera";
                miNormal.Text = "Normale";
                miSkin.Text = "Pelle";

                miGuide.Text = "Guida";
            }
            else if (lang.Equals("es")) //WIP
            {
                miFile.Text = "File";
                miStart.Text = "Empezar";
                miPause.Text = "Pausa";

                miApplication.Text = "Aplicación";
                miSettings.Text = "Ajustes";
                miLanguage.Text = "Idioma";
                miEnglish.Text = "Ingles";
                miItalian.Text = "Italiano";
                miSpanish.Text = "Español";
                miCamera.Text = "Camera";
                miCameraMode.Text = "Modo Cámara";
                miNormal.Text = "Normal";
                miSkin.Text = "Piel";

                miGuide.Text = "Guía";
            }
        }

        public void SetCamIndex(object sender, System.EventArgs e)
        {
            camIndex = Convert.ToInt16("" + sender.ToString()[0]);

            if (capture.IsRunning)
                capture.Stop();

            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            capture = new VideoCaptureDevice(videoDevices[camIndex].MonikerString);
            capture.NewFrame += new NewFrameEventHandler(VideoCaptureDevice_NewFrame);
        }

        private void ExtractContourAndHull(Image<Gray, byte> skin)
        {
            using (MemStorage storage = new MemStorage())
            {
                Contour<Point> contours = skin.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);
                Contour<Point> biggestContour = null;

                Double Result1 = 0;
                Double Result2 = 0;
                while (contours != null)
                {
                    Result1 = contours.Area;
                    if (Result1 > Result2)
                    {
                        Result2 = Result1;
                        biggestContour = contours;
                    }
                    contours = contours.HNext;
                }

                if (biggestContour != null)
                {
                    //currentFrame.Draw(biggestContour, new Bgr(Color.DarkViolet), 2);
                    Contour<Point> currentContour = biggestContour.ApproxPoly(biggestContour.Perimeter * 0.0025, storage);
                    currentFrame.Draw(currentContour, new Bgr(Color.LimeGreen), 2); //Contorno Verde
                    biggestContour = currentContour;

                    hull = biggestContour.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
                    box = biggestContour.GetMinAreaRect();
                    PointF[] points = box.GetVertices();
                    //handRect = box.MinAreaRect();
                    //currentFrame.Draw(handRect, new Bgr(200, 0, 0), 1);

                    Point[] ps = new Point[points.Length];
                    for (int i = 0; i < points.Length; i++)
                        ps[i] = new Point((int)points[i].X, (int)points[i].Y);

                    currentFrame.DrawPolyline(hull.ToArray(), true, new Bgr(200, 125, 75), 2);//Contorno Blu
                    currentFrame.Draw(new CircleF(new PointF(box.center.X, box.center.Y), 3), new Bgr(200, 125, 75), 2); //Punto centrale

                    //ellip.MCvBox2D= CvInvoke.cvFitEllipse2(biggestContour.Ptr);
                    //currentFrame.Draw(new Ellipse(ellip.MCvBox2D), new Bgr(Color.LavenderBlush), 3);

                    PointF center;
                    float radius;
                    //CvInvoke.cvMinEnclosingCircle(biggestContour.Ptr, out  center, out  radius);
                    //currentFrame.Draw(new CircleF(center, radius), new Bgr(Color.Gold), 2);

                    //currentFrame.Draw(new CircleF(new PointF(ellip.MCvBox2D.center.X, ellip.MCvBox2D.center.Y), 3), new Bgr(100, 25, 55), 2);
                    //currentFrame.Draw(ellip, new Bgr(Color.DeepPink), 2);

                    //CvInvoke.cvEllipse(currentFrame, new Point((int)ellip.MCvBox2D.center.X, (int)ellip.MCvBox2D.center.Y), new System.Drawing.Size((int)ellip.MCvBox2D.size.Width, (int)ellip.MCvBox2D.size.Height), ellip.MCvBox2D.angle, 0, 360, new MCvScalar(120, 233, 88), 1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, 0);
                    //currentFrame.Draw(new Ellipse(new PointF(box.center.X, box.center.Y), new SizeF(box.size.Height, box.size.Width), box.angle), new Bgr(0, 0, 0), 2);

                    filteredHull = new Seq<Point>(storage);
                    for (int i = 0; i < hull.Total; i++)
                    {
                        if (Math.Sqrt(Math.Pow(hull[i].X - hull[i + 1].X, 2) + Math.Pow(hull[i].Y - hull[i + 1].Y, 2)) > box.size.Width / 10)
                        {
                            filteredHull.Push(hull[i]);
                        }
                    }

                    defects = biggestContour.GetConvexityDefacts(storage, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
                    defectArray = defects.ToArray();

                    checkHandPosition(box.center.X, box.center.Y);
                }
            }
        }

        private void checkHandPosition(float centerX, float centerY)
        {
            short currentPerch = 100;
            short sendKey = 0; // 0 = No, 1 = RIGHT, 2 = LEFT

            if (!previousHandPosition.IsEmpty)
            {
                Console.WriteLine("Previous: " + previousHandPosition.ToString() + " Current: " + (new PointF(centerX, centerY)).ToString()); //Console debug

                if (centerX > previousHandPosition.X)
                {
                    currentPerch = (short)(100 - previousHandPosition.X / centerX * 100);

                    if (currentPerch > CHANGEPERC)
                    {
                        Console.WriteLine("Greater than " + CHANGEPERC + "%"); //Console debug
                        sendKey = 1;
                    }
                }
                else if (centerX < previousHandPosition.X)
                {
                    currentPerch = (short)(100 - centerX / previousHandPosition.X * 100);

                    if (currentPerch > CHANGEPERC)
                    {
                        Console.WriteLine("Greater than " + CHANGEPERC + "%"); //Console debug
                        sendKey = 2;
                    }
                }
                else
                    currentPerch = CHANGEPERC;
            }

            if (previousPerch < CHANGEPERC && selectedApp != null)
            {
                

                if (sendKey == 1)
                {
                    SetForegroundWindow(selectedApp.MainWindowHandle);
                    Thread.Sleep(18);

                    SendKeys.SendWait("{RIGHT}");
                    Console.WriteLine("RIGHT"); //Console debug

                    SetForegroundWindow(thisApp.MainWindowHandle);
                }
                else if (sendKey == 2)
                {
                    SetForegroundWindow(selectedApp.MainWindowHandle);
                    Thread.Sleep(18);

                    SendKeys.SendWait("{LEFT}");
                    Console.WriteLine("LEFT"); //Console debug

                    SetForegroundWindow(thisApp.MainWindowHandle);
                }
            }

            previousPerch = currentPerch;
            previousHandPosition = new PointF(centerX, centerY);
        }

        // File
        private void miStart_Click(object sender, EventArgs e)
        {
            lErrorLog.Text = "";
            lErrorLog.ForeColor = Color.Red;

            try
            {
                if (capture == null)
                {
                    FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    capture = new VideoCaptureDevice(videoDevices[camIndex].MonikerString);
                    capture.NewFrame += new NewFrameEventHandler(VideoCaptureDevice_NewFrame);
                }

                capture.Start();

                miFps.Visible = true;
            }
            catch (Exception ex)
            {
                if (lang.Equals("en"))
                    lErrorLog.Text = "Error: Camera not found,"
                        + Environment.NewLine + ex.Message;
                else if (lang.Equals("it"))
                    lErrorLog.Text = "Errore: La camera non è stata trovata,"
                        + Environment.NewLine + ex.Message;
                else if (lang.Equals("es"))
                    lErrorLog.Text = "Error: No se encontró la cámara,"
                        + Environment.NewLine + ex.Message;

                if (capture != null && capture.IsRunning)
                    capture.Stop();
            }
        }

        private void miPause_Click(object sender, EventArgs e)
        {
            if (capture != null && capture.IsRunning)
                capture.SignalToStop();
        }

        // Settings: Language
        private void miEnglish_Click(object sender, EventArgs e)
        {
            lang = "en";
            ChangeLocalization();
        }

        private void miItalian_Click(object sender, EventArgs e)
        {
            lang = "it";
            ChangeLocalization();
        }

        private void miSpanish_Click(object sender, EventArgs e)
        {
            lang = "es";
            ChangeLocalization();
        }

        private void miNormal_Click(object sender, EventArgs e)
        {
            cameraMode = 0;
        }

        private void miSkin_Click(object sender, EventArgs e)
        {
            cameraMode = 1;
        }

        private void tCheckApplications_Tick(object sender, EventArgs e)
        {
            if (isApplicationsListChanged())
                UpdateRunningApplications(true);
            if (connecteCameras.Count != GetAllConnectedCameras().Count())
                UpdateConnectedCameras(true);
        }

        private void fHandGestureRecognition_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (capture != null && capture.IsRunning)
                capture.Stop();
        }

        private void miGuide_Click(object sender, EventArgs e)
        {
            lErrorLog.ForeColor = Color.Black;

            if (lang.Equals("en"))
                lErrorLog.Text =
                "1. Select the application desired to work with Hand Gesture Recognition in Settings/Application." + Environment.NewLine +
                "2. Start \"Hand Gesture Recognition\" clicking File/Start." + Environment.NewLine +
                "If \"Hand Gesture Recognition\" doesn't find your camera select your camera in Settings/Camera." + Environment.NewLine +
                "3. Move your rapidly hand left and right to do actions." + Environment.NewLine +
                "Move left = left arrow key, move right = right arrow key." + Environment.NewLine +
                "4. When finished click File/Pause or exit \"Hand Gesture Recognition\".";
            else if (lang.Equals("it"))
                lErrorLog.Text =
                "1. Selezionare l'applicazione che si desidera utilizzare con il riconoscimento dei gesti delle mani in Impostazioni/Applicazione." + Environment.NewLine +
                "2. Avviare \"Hand Gesture Recognition\" facendo clic su File/Avvia." + Environment.NewLine +
                "Se \"Hand Gesture Recognition\" non trova la camera, selezionala in Impostazioni/Camera." + Environment.NewLine +
                "3. Muovi rapidamente la mano a sinistra ea destra per compiere azioni." + Environment.NewLine +
                "Muovi a sinistra = Tasto freccia sinistra, muovi a destra = Tasto freccia destra." + Environment.NewLine +
                "4. Al termine, fare clic su File/Pausa o uscire da \"Hand Gesture Recognition\".";
            else if (lang.Equals("es"))
                lErrorLog.Text =
                "1. Seleccione la aplicación que desee para trabajar con el reconocimiento de gestos con las manos en Configuración/Aplicación." + Environment.NewLine +
                "2. Inicie \"Hand Gesture Recognition\" haciendo clic en File/Iniciar." + Environment.NewLine +
                "Si \"Hand Gesture Recognition\" no encuentra su cámara, seleccione su cámara en Configuración/Cámara." + Environment.NewLine +
                "3. Mueva su mano rápidamente hacia la izquierda y hacia la derecha para realizar acciones." + Environment.NewLine +
                "Mover a la izquierda = tecla de flecha izquierda, mover a la derecha = tecla de flecha derecha." + Environment.NewLine +
                "4. Cuando haya terminado, haga clic en Archivo/Pausa o salga de \"Hand Gesture Recognition\".";
        }
    }
}