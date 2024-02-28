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
            this.pb_logo = new System.Windows.Forms.PictureBox();
            this.lbl_copyright = new System.Windows.Forms.Label();
            this.lbl_title = new System.Windows.Forms.Label();
            this.lbl_version = new System.Windows.Forms.Label();
            this.lbl_progression = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_logo)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_logo
            // 
            this.pb_logo.BackColor = System.Drawing.Color.Transparent;
            this.pb_logo.Image = global::Client.Resources.Logo;
            this.pb_logo.Location = new System.Drawing.Point(12, 50);
            this.pb_logo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pb_logo.Name = "pb_logo";
            this.pb_logo.Size = new System.Drawing.Size(698, 304);
            this.pb_logo.TabIndex = 0;
            this.pb_logo.TabStop = false;
            // 
            // lbl_copyright
            // 
            this.lbl_copyright.AutoSize = true;
            this.lbl_copyright.Location = new System.Drawing.Point(415, 379);
            this.lbl_copyright.Name = "lbl_copyright";
            this.lbl_copyright.Size = new System.Drawing.Size(295, 20);
            this.lbl_copyright.TabIndex = 1;
            this.lbl_copyright.Text = "© Unreal Technologies xxxx";
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.BackColor = System.Drawing.Color.Transparent;
            this.lbl_title.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_title.Location = new System.Drawing.Point(230, 9);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(282, 41);
            this.lbl_title.TabIndex = 2;
            this.lbl_title.Text = "Data Manager";
            // 
            // lbl_version
            // 
            this.lbl_version.AutoSize = true;
            this.lbl_version.Location = new System.Drawing.Point(12, 379);
            this.lbl_version.Name = "lbl_version";
            this.lbl_version.Size = new System.Drawing.Size(174, 20);
            this.lbl_version.TabIndex = 3;
            this.lbl_version.Text = "Version x.x.x.x";
            // 
            // lbl_progression
            // 
            this.lbl_progression.AutoSize = true;
            this.lbl_progression.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbl_progression.Location = new System.Drawing.Point(341, 356);
            this.lbl_progression.Name = "lbl_progression";
            this.lbl_progression.Size = new System.Drawing.Size(42, 20);
            this.lbl_progression.TabIndex = 4;
            this.lbl_progression.Text = "---";
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 411);
            this.Controls.Add(this.lbl_progression);
            this.Controls.Add(this.lbl_version);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.lbl_copyright);
            this.Controls.Add(this.pb_logo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Splash";
            this.Text = "Splash";
            this.Load += new System.EventHandler(this.Splash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pb_logo;
        private Label lbl_copyright;
        private Label lbl_title;
        private Label lbl_version;
        private Label lbl_progression;
    }
}