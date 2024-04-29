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
            // Splash
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(945, 509);
            Controls.Add(logo1);
            Name = "Splash";
            Text = "Splash";
            Controls.SetChildIndex(logo1, 0);
            ((System.ComponentModel.ISupportInitialize)logo1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private UT.Data.Controls.Logo logo1;
    }
}