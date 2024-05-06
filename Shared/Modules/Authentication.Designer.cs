using System.Drawing;
using System.Windows.Forms;
using UT.Data.Controls.Validated;

namespace Shared.Modules
{
    public partial class Authentication
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
            lbl_username = new Label();
            lbl_password = new Label();
            tb_username = new ValidatedTextBox();
            tb_password = new ValidatedTextBox();
            btn_login = new Button();
            SuspendLayout();
            // 
            // lbl_username
            // 
            lbl_username.AutoSize = true;
            lbl_username.Location = new Point(56, 73);
            lbl_username.Name = "lbl_username";
            lbl_username.Size = new Size(108, 20);
            lbl_username.TabIndex = 0;
            lbl_username.Text = "Username:";
            // 
            // lbl_password
            // 
            lbl_password.AutoSize = true;
            lbl_password.Location = new Point(56, 106);
            lbl_password.Name = "lbl_password";
            lbl_password.Size = new Size(108, 20);
            lbl_password.TabIndex = 1;
            lbl_password.Text = "Password:";
            // 
            // tb_username
            // 
            tb_username.IsRequired = true;
            tb_username.Location = new Point(179, 66);
            tb_username.Name = "tb_username";
            tb_username.Size = new Size(200, 27);
            tb_username.TabIndex = 2;
            // 
            // tb_password
            // 
            tb_password.IsRequired = true;
            tb_password.Location = new Point(179, 99);
            tb_password.Name = "tb_password";
            tb_password.Size = new Size(200, 27);
            tb_password.TabIndex = 3;
            // 
            // btn_login
            // 
            btn_login.Location = new Point(179, 132);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(177, 34);
            btn_login.TabIndex = 4;
            btn_login.Text = "Login";
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += Btn_login_Click;
            // 
            // Authentication
            // 
            ClientSize = new Size(400, 182);
            Controls.Add(btn_login);
            Controls.Add(tb_password);
            Controls.Add(tb_username);
            Controls.Add(lbl_password);
            Controls.Add(lbl_username);
            Name = "Authentication";
            Controls.SetChildIndex(lbl_username, 0);
            Controls.SetChildIndex(lbl_password, 0);
            Controls.SetChildIndex(tb_username, 0);
            Controls.SetChildIndex(tb_password, 0);
            Controls.SetChildIndex(btn_login, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl_username, lbl_password;
        private ValidatedTextBox tb_username, tb_password;
        private Button btn_login;
    }
}
