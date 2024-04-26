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
            logo2 = new UT.Data.Controls.Logo();
            ((System.ComponentModel.ISupportInitialize)logo2).BeginInit();
            SuspendLayout();
            // 
            // logo2
            // 
            logo2.Dock = DockStyle.Fill;
            logo2.Image = (Image)resources.GetObject("logo2.Image");
            logo2.Location = new Point(0, 0);
            logo2.Name = "logo2";
            logo2.Size = new Size(1024, 398);
            logo2.SizeMode = PictureBoxSizeMode.StretchImage;
            logo2.TabIndex = 0;
            logo2.TabStop = false;
            // 
            // Splash
            // 
            AutoScaleDimensions = new SizeF(11F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 398);
            Controls.Add(logo2);
            Name = "Splash";
            Text = "Splash";
            ((System.ComponentModel.ISupportInitialize)logo2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private UT.Data.Controls.Logo logo1;
        private UT.Data.Controls.Logo logo2;
    }
}