namespace Client
{
    partial class Splash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            logo1 = new UT.Data.Controls.Logo();
            gdiCopyright = new UT.Data.Controls.GdiText();
            gdiVersion = new UT.Data.Controls.GdiText();
            gdiContent = new UT.Data.Controls.GdiText();
            ((System.ComponentModel.ISupportInitialize)logo1).BeginInit();
            SuspendLayout();
            // 
            // logo1
            // 
            logo1.Image = (Image)resources.GetObject("logo1.Image");
            logo1.Location = new Point(0, 50);
            logo1.Name = "logo1";
            logo1.Size = new Size(945, 459);
            logo1.SizeMode = PictureBoxSizeMode.StretchImage;
            logo1.TabIndex = 1;
            logo1.TabStop = false;
            // 
            // gdiCopyright
            // 
            gdiCopyright.BackColor = Color.Transparent;
            gdiCopyright.BackgroundColor = Color.Gray;
            gdiCopyright.DrawBackground = false;
            gdiCopyright.DrawShadow = false;
            gdiCopyright.HorizontalAlignment = StringAlignment.Near;
            gdiCopyright.Location = new Point(12, 475);
            gdiCopyright.Name = "gdiCopyright";
            gdiCopyright.Opacity = 127;
            gdiCopyright.Shadow = UT.Data.Controls.GdiText.Shadows.BottomRight;
            gdiCopyright.Size = new Size(362, 22);
            gdiCopyright.TabIndex = 3;
            gdiCopyright.VerticalAlignment = StringAlignment.Near;
            // 
            // gdiVersion
            // 
            gdiVersion.BackColor = Color.Transparent;
            gdiVersion.BackgroundColor = Color.Gray;
            gdiVersion.DrawBackground = false;
            gdiVersion.DrawShadow = false;
            gdiVersion.HorizontalAlignment = StringAlignment.Far;
            gdiVersion.Location = new Point(571, 476);
            gdiVersion.Name = "gdiVersion";
            gdiVersion.Opacity = 127;
            gdiVersion.Shadow = UT.Data.Controls.GdiText.Shadows.BottomRight;
            gdiVersion.Size = new Size(362, 22);
            gdiVersion.TabIndex = 4;
            gdiVersion.VerticalAlignment = StringAlignment.Near;
            // 
            // gdiContent
            // 
            gdiContent.BackColor = Color.Transparent;
            gdiContent.BackgroundColor = Color.Gray;
            gdiContent.DrawBackground = true;
            gdiContent.DrawShadow = true;
            gdiContent.Font = new Font("Courier New", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gdiContent.ForeColor = Color.White;
            gdiContent.HorizontalAlignment = StringAlignment.Center;
            gdiContent.Location = new Point(12, 438);
            gdiContent.Name = "gdiContent";
            gdiContent.Opacity = 127;
            gdiContent.Shadow = UT.Data.Controls.GdiText.Shadows.BottomRight;
            gdiContent.Size = new Size(921, 32);
            gdiContent.TabIndex = 5;
            gdiContent.VerticalAlignment = StringAlignment.Center;
            // 
            // Splash
            // 
            AutoScaleDimensions = new SizeF(11F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(945, 509);
            Controls.Add(gdiContent);
            Controls.Add(gdiVersion);
            Controls.Add(gdiCopyright);
            Controls.Add(logo1);
            Name = "Splash";
            Text = "Splash";
            Controls.SetChildIndex(logo1, 0);
            Controls.SetChildIndex(gdiCopyright, 0);
            Controls.SetChildIndex(gdiVersion, 0);
            Controls.SetChildIndex(gdiContent, 0);
            ((System.ComponentModel.ISupportInitialize)logo1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private UT.Data.Controls.Logo logo1;
        private UT.Data.Controls.GdiText gdiCopyright;
        private UT.Data.Controls.GdiText gdiVersion;
        private UT.Data.Controls.GdiText gdiContent;
    }
}