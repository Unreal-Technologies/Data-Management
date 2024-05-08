using System.Drawing;

namespace Shared.Modules
{
    public partial class Main
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
            menuStrip = new System.Windows.Forms.MenuStrip();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Dock = System.Windows.Forms.DockStyle.Left;
            menuStrip.ImageScalingSize = new Size(24, 24);
            menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(186, 719);
            menuStrip.TabIndex = 2;
            menuStrip.Text = "menuStrip";
            // 
            // Main
            // 
            ClientSize = new Size(1114, 719);
            Controls.Add(menuStrip);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip;
            Name = "Main";
            TransparencyKey = Color.Empty;
            Controls.SetChildIndex(menuStrip, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
    }
}
