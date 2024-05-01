namespace Client.ServerConfiguration
{
    partial class Form
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
            this.lbl_serverIP = new System.Windows.Forms.Label();
            this.lbl_serverPort = new System.Windows.Forms.Label();
            this.tb_serverIP = new System.Windows.Forms.TextBox();
            this.nud_serverPort = new System.Windows.Forms.NumericUpDown();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.lbl_computerLeft = new System.Windows.Forms.Label();
            this.lbl_computerRight = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nud_serverPort)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_serverIP
            // 
            this.lbl_serverIP.AutoSize = true;
            this.lbl_serverIP.Location = new System.Drawing.Point(13, 35);
            this.lbl_serverIP.Name = "lbl_serverIP";
            this.lbl_serverIP.Size = new System.Drawing.Size(119, 20);
            this.lbl_serverIP.TabIndex = 1;
            this.lbl_serverIP.Text = "Server IP:";
            // 
            // lbl_serverPort
            // 
            this.lbl_serverPort.AutoSize = true;
            this.lbl_serverPort.Location = new System.Drawing.Point(12, 68);
            this.lbl_serverPort.Name = "lbl_serverPort";
            this.lbl_serverPort.Size = new System.Drawing.Size(141, 20);
            this.lbl_serverPort.TabIndex = 2;
            this.lbl_serverPort.Text = "Server Port:";
            // 
            // tb_serverIP
            // 
            this.tb_serverIP.Location = new System.Drawing.Point(161, 32);
            this.tb_serverIP.Name = "tb_serverIP";
            this.tb_serverIP.Size = new System.Drawing.Size(206, 28);
            this.tb_serverIP.TabIndex = 3;
            this.tb_serverIP.Text = "xxx.xxx.xxx.xxx";
            // 
            // nud_serverPort
            // 
            this.nud_serverPort.Location = new System.Drawing.Point(161, 66);
            this.nud_serverPort.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nud_serverPort.Name = "nud_serverPort";
            this.nud_serverPort.Size = new System.Drawing.Size(206, 28);
            this.nud_serverPort.TabIndex = 4;
            this.nud_serverPort.Value = new decimal(new int[] {
            1404,
            0,
            0,
            0});
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(13, 100);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(112, 34);
            this.btn_cancel.TabIndex = 5;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.Btn_cancel_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(255, 100);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(112, 34);
            this.btn_save.TabIndex = 6;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.Btn_save_Click);
            // 
            // lbl_computerLeft
            // 
            this.lbl_computerLeft.AutoSize = true;
            this.lbl_computerLeft.Location = new System.Drawing.Point(12, 9);
            this.lbl_computerLeft.Name = "lbl_computerLeft";
            this.lbl_computerLeft.Size = new System.Drawing.Size(108, 20);
            this.lbl_computerLeft.TabIndex = 7;
            this.lbl_computerLeft.Text = "Computer:";
            // 
            // lbl_computerRight
            // 
            this.lbl_computerRight.AutoSize = true;
            this.lbl_computerRight.Location = new System.Drawing.Point(161, 9);
            this.lbl_computerRight.Name = "lbl_computerRight";
            this.lbl_computerRight.Size = new System.Drawing.Size(42, 20);
            this.lbl_computerRight.TabIndex = 8;
            this.lbl_computerRight.Text = "xxx";
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 146);
            this.Controls.Add(this.lbl_computerRight);
            this.Controls.Add(this.lbl_computerLeft);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.nud_serverPort);
            this.Controls.Add(this.tb_serverIP);
            this.Controls.Add(this.lbl_serverPort);
            this.Controls.Add(this.lbl_serverIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form";
            this.Text = "Server Configuration";
            this.Load += new System.EventHandler(this.Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nud_serverPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label lbl_serverIP;
        private Label lbl_serverPort;
        private TextBox tb_serverIP;
        private NumericUpDown nud_serverPort;
        private Button btn_cancel;
        private Button btn_save;
        private Label lbl_computerLeft;
        private Label lbl_computerRight;
    }
}