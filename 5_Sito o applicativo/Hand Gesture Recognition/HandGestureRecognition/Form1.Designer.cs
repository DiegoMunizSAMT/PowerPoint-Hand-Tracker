namespace HandGestureRecognition
{
    partial class fHandGestureRecognition
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fHandGestureRecognition));
            this.lFps = new System.Windows.Forms.Label();
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miStart = new System.Windows.Forms.ToolStripMenuItem();
            this.miPause = new System.Windows.Forms.ToolStripMenuItem();
            this.miSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.miApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.miLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.miEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.miItalian = new System.Windows.Forms.ToolStripMenuItem();
            this.miSpanish = new System.Windows.Forms.ToolStripMenuItem();
            this.miCamera = new System.Windows.Forms.ToolStripMenuItem();
            this.miCameraMode = new System.Windows.Forms.ToolStripMenuItem();
            this.miNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.miSkin = new System.Windows.Forms.ToolStripMenuItem();
            this.miGuide = new System.Windows.Forms.ToolStripMenuItem();
            this.miFps = new System.Windows.Forms.ToolStripMenuItem();
            this.tCheckApplications = new System.Windows.Forms.Timer(this.components);
            this.lErrorLog = new System.Windows.Forms.Label();
            this.pbCurrentFrame = new System.Windows.Forms.PictureBox();
            this.msMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrentFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // lFps
            // 
            this.lFps.AutoSize = true;
            this.lFps.Location = new System.Drawing.Point(12, 22);
            this.lFps.Name = "lFps";
            this.lFps.Size = new System.Drawing.Size(0, 13);
            this.lFps.TabIndex = 14;
            // 
            // msMenu
            // 
            this.msMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miSettings,
            this.miGuide,
            this.miFps});
            this.msMenu.Location = new System.Drawing.Point(0, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.msMenu.Size = new System.Drawing.Size(666, 24);
            this.msMenu.TabIndex = 13;
            this.msMenu.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miStart,
            this.miPause});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 20);
            this.miFile.Text = "File";
            // 
            // miStart
            // 
            this.miStart.Name = "miStart";
            this.miStart.Size = new System.Drawing.Size(180, 22);
            this.miStart.Text = "Start";
            this.miStart.Click += new System.EventHandler(this.miStart_Click);
            // 
            // miPause
            // 
            this.miPause.Name = "miPause";
            this.miPause.Size = new System.Drawing.Size(180, 22);
            this.miPause.Text = "Pause";
            this.miPause.Click += new System.EventHandler(this.miPause_Click);
            // 
            // miSettings
            // 
            this.miSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miApplication,
            this.miLanguage,
            this.miCamera,
            this.miCameraMode});
            this.miSettings.Name = "miSettings";
            this.miSettings.Size = new System.Drawing.Size(61, 20);
            this.miSettings.Text = "Settings";
            // 
            // miApplication
            // 
            this.miApplication.Name = "miApplication";
            this.miApplication.Size = new System.Drawing.Size(180, 22);
            this.miApplication.Text = "Application";
            // 
            // miLanguage
            // 
            this.miLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEnglish,
            this.miItalian,
            this.miSpanish});
            this.miLanguage.Name = "miLanguage";
            this.miLanguage.Size = new System.Drawing.Size(180, 22);
            this.miLanguage.Text = "Language";
            // 
            // miEnglish
            // 
            this.miEnglish.Name = "miEnglish";
            this.miEnglish.Size = new System.Drawing.Size(115, 22);
            this.miEnglish.Text = "English";
            this.miEnglish.Click += new System.EventHandler(this.miEnglish_Click);
            // 
            // miItalian
            // 
            this.miItalian.Name = "miItalian";
            this.miItalian.Size = new System.Drawing.Size(115, 22);
            this.miItalian.Text = "Italian";
            this.miItalian.Click += new System.EventHandler(this.miItalian_Click);
            // 
            // miSpanish
            // 
            this.miSpanish.Name = "miSpanish";
            this.miSpanish.Size = new System.Drawing.Size(115, 22);
            this.miSpanish.Text = "Spanish";
            this.miSpanish.Click += new System.EventHandler(this.miSpanish_Click);
            // 
            // miCamera
            // 
            this.miCamera.Name = "miCamera";
            this.miCamera.Size = new System.Drawing.Size(180, 22);
            this.miCamera.Text = "Camera";
            // 
            // miCameraMode
            // 
            this.miCameraMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNormal,
            this.miSkin});
            this.miCameraMode.Name = "miCameraMode";
            this.miCameraMode.Size = new System.Drawing.Size(180, 22);
            this.miCameraMode.Text = "Camera Mode";
            // 
            // miNormal
            // 
            this.miNormal.Name = "miNormal";
            this.miNormal.Size = new System.Drawing.Size(114, 22);
            this.miNormal.Text = "Normal";
            this.miNormal.Click += new System.EventHandler(this.miNormal_Click);
            // 
            // miSkin
            // 
            this.miSkin.Name = "miSkin";
            this.miSkin.Size = new System.Drawing.Size(114, 22);
            this.miSkin.Text = "Skin";
            this.miSkin.Click += new System.EventHandler(this.miSkin_Click);
            // 
            // miGuide
            // 
            this.miGuide.Name = "miGuide";
            this.miGuide.Size = new System.Drawing.Size(50, 20);
            this.miGuide.Text = "Guide";
            this.miGuide.Click += new System.EventHandler(this.miGuide_Click);
            // 
            // miFps
            // 
            this.miFps.Enabled = false;
            this.miFps.Name = "miFps";
            this.miFps.Size = new System.Drawing.Size(40, 20);
            this.miFps.Text = "Fps:";
            this.miFps.Visible = false;
            // 
            // tCheckApplications
            // 
            this.tCheckApplications.Interval = 1200;
            this.tCheckApplications.Tick += new System.EventHandler(this.tCheckApplications_Tick);
            // 
            // lErrorLog
            // 
            this.lErrorLog.AutoSize = true;
            this.lErrorLog.ForeColor = System.Drawing.Color.Firebrick;
            this.lErrorLog.Location = new System.Drawing.Point(12, 38);
            this.lErrorLog.Name = "lErrorLog";
            this.lErrorLog.Size = new System.Drawing.Size(49, 13);
            this.lErrorLog.TabIndex = 15;
            this.lErrorLog.Text = "lErrorLog";
            // 
            // pbCurrentFrame
            // 
            this.pbCurrentFrame.Location = new System.Drawing.Point(15, 38);
            this.pbCurrentFrame.Name = "pbCurrentFrame";
            this.pbCurrentFrame.Size = new System.Drawing.Size(640, 480);
            this.pbCurrentFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbCurrentFrame.TabIndex = 18;
            this.pbCurrentFrame.TabStop = false;
            // 
            // fHandGestureRecognition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 527);
            this.Controls.Add(this.lErrorLog);
            this.Controls.Add(this.pbCurrentFrame);
            this.Controls.Add(this.lFps);
            this.Controls.Add(this.msMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(686, 570);
            this.MinimumSize = new System.Drawing.Size(686, 570);
            this.Name = "fHandGestureRecognition";
            this.Text = "Hand Gesture Recognition";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fHandGestureRecognition_FormClosed);
            this.Load += new System.EventHandler(this.fHandGestureReconition_Load);
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrentFrame)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lFps;
        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miStart;
        private System.Windows.Forms.ToolStripMenuItem miPause;
        private System.Windows.Forms.ToolStripMenuItem miSettings;
        private System.Windows.Forms.ToolStripMenuItem miApplication;
        private System.Windows.Forms.ToolStripMenuItem miLanguage;
        private System.Windows.Forms.ToolStripMenuItem miEnglish;
        private System.Windows.Forms.ToolStripMenuItem miItalian;
        private System.Windows.Forms.ToolStripMenuItem miCamera;
        private System.Windows.Forms.ToolStripMenuItem miGuide;
        private System.Windows.Forms.ToolStripMenuItem miFps;
        private System.Windows.Forms.Timer tCheckApplications;
        private System.Windows.Forms.Label lErrorLog;
        private System.Windows.Forms.PictureBox pbCurrentFrame;
        private System.Windows.Forms.ToolStripMenuItem miCameraMode;
        private System.Windows.Forms.ToolStripMenuItem miNormal;
        private System.Windows.Forms.ToolStripMenuItem miSkin;
        private System.Windows.Forms.ToolStripMenuItem miSpanish;
    }
}

