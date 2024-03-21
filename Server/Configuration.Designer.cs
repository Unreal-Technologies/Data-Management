namespace Server
{
    partial class Configuration
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
            gb_server = new System.Windows.Forms.GroupBox();
            gb_server_lbl_device_dynamic = new System.Windows.Forms.Label();
            gb_server_lbl_device_static = new System.Windows.Forms.Label();
            gb_server_vnud_port_dynamic = new UT.Data.Controls.Validated.ValidatedNumericUpDown();
            gb_server_lbl_port_static = new System.Windows.Forms.Label();
            gb_database = new System.Windows.Forms.GroupBox();
            gb_database_vcb_type_dynamic = new UT.Data.Controls.Validated.ValidatedComboBox();
            gb_database_vnud_port_dynamic = new UT.Data.Controls.Validated.ValidatedNumericUpDown();
            gb_database_lbl_port_static = new System.Windows.Forms.Label();
            gb_database_vtb_database_dynamic = new UT.Data.Controls.Validated.ValidatedTextBox();
            gb_database_vtb_password_dynamic = new UT.Data.Controls.Validated.ValidatedTextBox();
            gb_database_vtb_username_dynamic = new UT.Data.Controls.Validated.ValidatedTextBox();
            gb_database_vipa_ip_dynamic = new UT.Data.Controls.Validated.ValidatedIpAddress();
            gb_database_lbl_username_static = new System.Windows.Forms.Label();
            gb_database_lbl_password_static = new System.Windows.Forms.Label();
            gb_database_lbl_database_static = new System.Windows.Forms.Label();
            gb_database_lbl_ip_static = new System.Windows.Forms.Label();
            gb_database_lbl_type_static = new System.Windows.Forms.Label();
            btn_save = new System.Windows.Forms.Button();
            gb_server.SuspendLayout();
            gb_database.SuspendLayout();
            SuspendLayout();
            // 
            // gb_server
            // 
            gb_server.Controls.Add(gb_server_lbl_device_dynamic);
            gb_server.Controls.Add(gb_server_lbl_device_static);
            gb_server.Controls.Add(gb_server_vnud_port_dynamic);
            gb_server.Controls.Add(gb_server_lbl_port_static);
            gb_server.Location = new System.Drawing.Point(8, 9);
            gb_server.Margin = new System.Windows.Forms.Padding(2);
            gb_server.Name = "gb_server";
            gb_server.Padding = new System.Windows.Forms.Padding(2);
            gb_server.Size = new System.Drawing.Size(221, 68);
            gb_server.TabIndex = 0;
            gb_server.TabStop = false;
            gb_server.Text = "Server";
            // 
            // gb_server_lbl_device_dynamic
            // 
            gb_server_lbl_device_dynamic.AutoSize = true;
            gb_server_lbl_device_dynamic.Enabled = false;
            gb_server_lbl_device_dynamic.Location = new System.Drawing.Point(80, 16);
            gb_server_lbl_device_dynamic.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            gb_server_lbl_device_dynamic.Name = "gb_server_lbl_device_dynamic";
            gb_server_lbl_device_dynamic.Size = new System.Drawing.Size(77, 15);
            gb_server_lbl_device_dynamic.TabIndex = 5;
            gb_server_lbl_device_dynamic.Text = "--device--";
            // 
            // gb_server_lbl_device_static
            // 
            gb_server_lbl_device_static.AutoSize = true;
            gb_server_lbl_device_static.Location = new System.Drawing.Point(18, 16);
            gb_server_lbl_device_static.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            gb_server_lbl_device_static.Name = "gb_server_lbl_device_static";
            gb_server_lbl_device_static.Size = new System.Drawing.Size(56, 15);
            gb_server_lbl_device_static.TabIndex = 4;
            gb_server_lbl_device_static.Text = "Device:";
            // 
            // gb_server_vnud_port_dynamic
            // 
            gb_server_vnud_port_dynamic.IsRequired = true;
            gb_server_vnud_port_dynamic.Location = new System.Drawing.Point(80, 39);
            gb_server_vnud_port_dynamic.Margin = new System.Windows.Forms.Padding(2);
            gb_server_vnud_port_dynamic.Name = "gb_server_vnud_port_dynamic";
            gb_server_vnud_port_dynamic.Size = new System.Drawing.Size(115, 21);
            gb_server_vnud_port_dynamic.TabIndex = 3;
            // 
            // gb_server_lbl_port_static
            // 
            gb_server_lbl_port_static.AutoSize = true;
            gb_server_lbl_port_static.Location = new System.Drawing.Point(32, 39);
            gb_server_lbl_port_static.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            gb_server_lbl_port_static.Name = "gb_server_lbl_port_static";
            gb_server_lbl_port_static.Size = new System.Drawing.Size(42, 15);
            gb_server_lbl_port_static.TabIndex = 3;
            gb_server_lbl_port_static.Text = "Port:";
            // 
            // gb_database
            // 
            gb_database.Controls.Add(gb_database_vcb_type_dynamic);
            gb_database.Controls.Add(gb_database_vnud_port_dynamic);
            gb_database.Controls.Add(gb_database_lbl_port_static);
            gb_database.Controls.Add(gb_database_vtb_database_dynamic);
            gb_database.Controls.Add(gb_database_vtb_password_dynamic);
            gb_database.Controls.Add(gb_database_vtb_username_dynamic);
            gb_database.Controls.Add(gb_database_vipa_ip_dynamic);
            gb_database.Controls.Add(gb_database_lbl_username_static);
            gb_database.Controls.Add(gb_database_lbl_password_static);
            gb_database.Controls.Add(gb_database_lbl_database_static);
            gb_database.Controls.Add(gb_database_lbl_ip_static);
            gb_database.Controls.Add(gb_database_lbl_type_static);
            gb_database.Location = new System.Drawing.Point(8, 81);
            gb_database.Margin = new System.Windows.Forms.Padding(2);
            gb_database.Name = "gb_database";
            gb_database.Padding = new System.Windows.Forms.Padding(2);
            gb_database.Size = new System.Drawing.Size(221, 174);
            gb_database.TabIndex = 1;
            gb_database.TabStop = false;
            gb_database.Text = "Database";
            // 
            // gb_database_vcb_type_dynamic
            // 
            gb_database_vcb_type_dynamic.IsRequired = true;
            gb_database_vcb_type_dynamic.Location = new System.Drawing.Point(79, 17);
            gb_database_vcb_type_dynamic.Name = "gb_database_vcb_type_dynamic";
            gb_database_vcb_type_dynamic.Size = new System.Drawing.Size(116, 24);
            gb_database_vcb_type_dynamic.TabIndex = 3;
            // 
            // gb_database_vnud_port_dynamic
            // 
            gb_database_vnud_port_dynamic.IsRequired = true;
            gb_database_vnud_port_dynamic.Location = new System.Drawing.Point(80, 69);
            gb_database_vnud_port_dynamic.Margin = new System.Windows.Forms.Padding(2);
            gb_database_vnud_port_dynamic.Name = "gb_database_vnud_port_dynamic";
            gb_database_vnud_port_dynamic.Size = new System.Drawing.Size(115, 21);
            gb_database_vnud_port_dynamic.TabIndex = 3;
            // 
            // gb_database_lbl_port_static
            // 
            gb_database_lbl_port_static.AutoSize = true;
            gb_database_lbl_port_static.Location = new System.Drawing.Point(32, 69);
            gb_database_lbl_port_static.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            gb_database_lbl_port_static.Name = "gb_database_lbl_port_static";
            gb_database_lbl_port_static.Size = new System.Drawing.Size(42, 15);
            gb_database_lbl_port_static.TabIndex = 5;
            gb_database_lbl_port_static.Text = "Port:";
            // 
            // gb_database_vtb_database_dynamic
            // 
            gb_database_vtb_database_dynamic.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            gb_database_vtb_database_dynamic.IsRequired = true;
            gb_database_vtb_database_dynamic.Location = new System.Drawing.Point(80, 139);
            gb_database_vtb_database_dynamic.Margin = new System.Windows.Forms.Padding(0);
            gb_database_vtb_database_dynamic.MinimumSize = new System.Drawing.Size(64, 21);
            gb_database_vtb_database_dynamic.Name = "gb_database_vtb_database_dynamic";
            gb_database_vtb_database_dynamic.Size = new System.Drawing.Size(115, 21);
            gb_database_vtb_database_dynamic.TabIndex = 11;
            // 
            // gb_database_vtb_password_dynamic
            // 
            gb_database_vtb_password_dynamic.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            gb_database_vtb_password_dynamic.IsRequired = false;
            gb_database_vtb_password_dynamic.Location = new System.Drawing.Point(80, 116);
            gb_database_vtb_password_dynamic.Margin = new System.Windows.Forms.Padding(0);
            gb_database_vtb_password_dynamic.MinimumSize = new System.Drawing.Size(64, 21);
            gb_database_vtb_password_dynamic.Name = "gb_database_vtb_password_dynamic";
            gb_database_vtb_password_dynamic.Size = new System.Drawing.Size(115, 21);
            gb_database_vtb_password_dynamic.TabIndex = 12;
            // 
            // gb_database_vtb_username_dynamic
            // 
            gb_database_vtb_username_dynamic.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            gb_database_vtb_username_dynamic.IsRequired = true;
            gb_database_vtb_username_dynamic.Location = new System.Drawing.Point(80, 92);
            gb_database_vtb_username_dynamic.Margin = new System.Windows.Forms.Padding(0);
            gb_database_vtb_username_dynamic.MinimumSize = new System.Drawing.Size(64, 21);
            gb_database_vtb_username_dynamic.Name = "gb_database_vtb_username_dynamic";
            gb_database_vtb_username_dynamic.Size = new System.Drawing.Size(115, 21);
            gb_database_vtb_username_dynamic.TabIndex = 11;
            // 
            // gb_database_vipa_ip_dynamic
            // 
            gb_database_vipa_ip_dynamic.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            gb_database_vipa_ip_dynamic.IsRequired = true;
            gb_database_vipa_ip_dynamic.Location = new System.Drawing.Point(80, 44);
            gb_database_vipa_ip_dynamic.Margin = new System.Windows.Forms.Padding(0);
            gb_database_vipa_ip_dynamic.MinimumSize = new System.Drawing.Size(64, 21);
            gb_database_vipa_ip_dynamic.Name = "gb_database_vipa_ip_dynamic";
            gb_database_vipa_ip_dynamic.Size = new System.Drawing.Size(115, 21);
            gb_database_vipa_ip_dynamic.TabIndex = 10;
            // 
            // gb_database_lbl_username_static
            // 
            gb_database_lbl_username_static.AutoSize = true;
            gb_database_lbl_username_static.Location = new System.Drawing.Point(4, 92);
            gb_database_lbl_username_static.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            gb_database_lbl_username_static.Name = "gb_database_lbl_username_static";
            gb_database_lbl_username_static.Size = new System.Drawing.Size(70, 15);
            gb_database_lbl_username_static.TabIndex = 9;
            gb_database_lbl_username_static.Text = "Username:";
            // 
            // gb_database_lbl_password_static
            // 
            gb_database_lbl_password_static.AutoSize = true;
            gb_database_lbl_password_static.Location = new System.Drawing.Point(4, 116);
            gb_database_lbl_password_static.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            gb_database_lbl_password_static.Name = "gb_database_lbl_password_static";
            gb_database_lbl_password_static.Size = new System.Drawing.Size(70, 15);
            gb_database_lbl_password_static.TabIndex = 8;
            gb_database_lbl_password_static.Text = "Password:";
            // 
            // gb_database_lbl_database_static
            // 
            gb_database_lbl_database_static.AutoSize = true;
            gb_database_lbl_database_static.Location = new System.Drawing.Point(4, 139);
            gb_database_lbl_database_static.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            gb_database_lbl_database_static.Name = "gb_database_lbl_database_static";
            gb_database_lbl_database_static.Size = new System.Drawing.Size(70, 15);
            gb_database_lbl_database_static.TabIndex = 7;
            gb_database_lbl_database_static.Text = "Database:";
            // 
            // gb_database_lbl_ip_static
            // 
            gb_database_lbl_ip_static.AutoSize = true;
            gb_database_lbl_ip_static.Location = new System.Drawing.Point(46, 44);
            gb_database_lbl_ip_static.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            gb_database_lbl_ip_static.Name = "gb_database_lbl_ip_static";
            gb_database_lbl_ip_static.Size = new System.Drawing.Size(28, 15);
            gb_database_lbl_ip_static.TabIndex = 3;
            gb_database_lbl_ip_static.Text = "IP:";
            // 
            // gb_database_lbl_type_static
            // 
            gb_database_lbl_type_static.AutoSize = true;
            gb_database_lbl_type_static.Location = new System.Drawing.Point(32, 20);
            gb_database_lbl_type_static.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            gb_database_lbl_type_static.Name = "gb_database_lbl_type_static";
            gb_database_lbl_type_static.Size = new System.Drawing.Size(42, 15);
            gb_database_lbl_type_static.TabIndex = 1;
            gb_database_lbl_type_static.Text = "Type:";
            // 
            // btn_save
            // 
            btn_save.Location = new System.Drawing.Point(87, 259);
            btn_save.Margin = new System.Windows.Forms.Padding(2);
            btn_save.Name = "btn_save";
            btn_save.Size = new System.Drawing.Size(116, 26);
            btn_save.TabIndex = 2;
            btn_save.Text = "Save";
            btn_save.UseVisualStyleBackColor = true;
            btn_save.Click += Btn_save_Click;
            // 
            // Configuration
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(238, 290);
            Controls.Add(btn_save);
            Controls.Add(gb_database);
            Controls.Add(gb_server);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(2);
            Name = "Configuration";
            Text = "Configuration";
            gb_server.ResumeLayout(false);
            gb_server.PerformLayout();
            gb_database.ResumeLayout(false);
            gb_database.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox gb_server;
        private System.Windows.Forms.Label gb_server_lbl_port_static;
        private System.Windows.Forms.GroupBox gb_database;
        private System.Windows.Forms.Label gb_database_lbl_ip_static;
        private System.Windows.Forms.Label gb_database_lbl_type_static;
        private System.Windows.Forms.Label gb_database_lbl_username_static;
        private System.Windows.Forms.Label gb_database_lbl_password_static;
        private System.Windows.Forms.Label gb_database_lbl_database_static;
        private System.Windows.Forms.Button btn_save;
        private UT.Data.Controls.Validated.ValidatedIpAddress gb_database_vipa_ip_dynamic;
        private UT.Data.Controls.Validated.ValidatedTextBox gb_database_vtb_database_dynamic;
        private UT.Data.Controls.Validated.ValidatedTextBox gb_database_vtb_password_dynamic;
        private UT.Data.Controls.Validated.ValidatedTextBox gb_database_vtb_username_dynamic;
        private System.Windows.Forms.Label gb_database_lbl_port_static;
        private UT.Data.Controls.Validated.ValidatedNumericUpDown gb_server_vnud_port_dynamic;
        private UT.Data.Controls.Validated.ValidatedNumericUpDown gb_database_vnud_port_dynamic;
        private UT.Data.Controls.Validated.ValidatedComboBox gb_database_vcb_type_dynamic;
        private System.Windows.Forms.Label gb_server_lbl_device_dynamic;
        private System.Windows.Forms.Label gb_server_lbl_device_static;
    }
}