namespace Client
{
    partial class AppConfiguration
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
            vnudPort = new UT.Data.Controls.Validated.ValidatedNumericUpDown();
            vtbTitle = new UT.Data.Controls.Validated.ValidatedTextBox();
            groupBox1 = new GroupBox();
            lblTitle = new Label();
            groupBox2 = new GroupBox();
            lblPort = new Label();
            lblIp = new Label();
            btnSave = new Button();
            vipaIp = new UT.Data.Controls.Validated.ValidatedIpAddress();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // vnudPort
            // 
            vnudPort.IsRequired = true;
            vnudPort.Location = new Point(96, 53);
            vnudPort.Name = "vnudPort";
            vnudPort.Size = new Size(200, 27);
            vnudPort.TabIndex = 2;
            // 
            // vtbTitle
            // 
            vtbTitle.IsRequired = true;
            vtbTitle.Location = new Point(96, 27);
            vtbTitle.Name = "vtbTitle";
            vtbTitle.Size = new Size(200, 27);
            vtbTitle.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lblTitle);
            groupBox1.Controls.Add(vtbTitle);
            groupBox1.Location = new Point(12, 56);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(313, 68);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Application";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(15, 34);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(75, 20);
            lblTitle.TabIndex = 3;
            lblTitle.Text = "Title:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(vipaIp);
            groupBox2.Controls.Add(lblPort);
            groupBox2.Controls.Add(lblIp);
            groupBox2.Controls.Add(vnudPort);
            groupBox2.Location = new Point(12, 130);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(313, 89);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Server";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(15, 60);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(64, 20);
            lblPort.TabIndex = 5;
            lblPort.Text = "Port:";
            // 
            // lblIp
            // 
            lblIp.AutoSize = true;
            lblIp.Location = new Point(15, 27);
            lblIp.Name = "lblIp";
            lblIp.Size = new Size(42, 20);
            lblIp.TabIndex = 4;
            lblIp.Text = "IP:";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(75, 228);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(200, 34);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save Changes";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;
            // 
            // vipaIp
            // 
            vipaIp.IsRequired = true;
            vipaIp.Location = new Point(96, 20);
            vipaIp.Name = "vipaIp";
            vipaIp.Size = new Size(200, 27);
            vipaIp.TabIndex = 6;
            // 
            // AppConfiguration
            // 
            AutoScaleDimensions = new SizeF(11F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(335, 274);
            Controls.Add(btnSave);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "AppConfiguration";
            Controls.SetChildIndex(groupBox1, 0);
            Controls.SetChildIndex(groupBox2, 0);
            Controls.SetChildIndex(btnSave, 0);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private UT.Data.Controls.Validated.ValidatedNumericUpDown vnudPort;
        private UT.Data.Controls.Validated.ValidatedTextBox vtbTitle;
        private GroupBox groupBox1;
        private Label lblTitle;
        private GroupBox groupBox2;
        private Button btnSave;
        private Label lblPort;
        private Label lblIp;
        private UT.Data.Controls.Validated.ValidatedIpAddress vipaIp;
    }
}