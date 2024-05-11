using System.Drawing;

namespace Shared.Modules
{
    public partial class Content
    {/// <summary>
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
            ofdUpload = new System.Windows.Forms.OpenFileDialog();
            tabControl = new System.Windows.Forms.TabControl();
            tabPage_upload = new System.Windows.Forms.TabPage();
            tabPage_upload_lbl_extension_v = new System.Windows.Forms.Label();
            tabPage_upload_lbl_extension = new System.Windows.Forms.Label();
            tabPage_upload_tb_path = new System.Windows.Forms.TextBox();
            tabPage_upload_btn_changeFile = new System.Windows.Forms.Button();
            tabPage_upload_btn_save = new System.Windows.Forms.Button();
            tabPage_upload_pb_image = new System.Windows.Forms.PictureBox();
            tabPage_upload_lbl_type_v = new System.Windows.Forms.Label();
            tabPage_upload_lbl_type = new System.Windows.Forms.Label();
            tabPage_upload_vtb_description = new UT.Data.Controls.Validated.ValidatedTextBox();
            tabPage_upload_lbl_description = new System.Windows.Forms.Label();
            tabPage_list = new System.Windows.Forms.TabPage();
            tabPage_delete = new System.Windows.Forms.TabPage();
            tabPage_delete_tb_id = new System.Windows.Forms.TextBox();
            tabPage_delete_btn_no = new System.Windows.Forms.Button();
            tabPage_delete_btn_yes = new System.Windows.Forms.Button();
            tabPage_delete_lbl_message = new System.Windows.Forms.Label();
            tabControl.SuspendLayout();
            tabPage_upload.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tabPage_upload_pb_image).BeginInit();
            tabPage_delete.SuspendLayout();
            SuspendLayout();
            // 
            // ofdUpload
            // 
            ofdUpload.Filter = "Any File|*.*";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPage_upload);
            tabControl.Controls.Add(tabPage_list);
            tabControl.Controls.Add(tabPage_delete);
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.Location = new Point(0, 50);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(2040, 1105);
            tabControl.TabIndex = 1;
            // 
            // tabPage_upload
            // 
            tabPage_upload.Controls.Add(tabPage_upload_lbl_extension_v);
            tabPage_upload.Controls.Add(tabPage_upload_lbl_extension);
            tabPage_upload.Controls.Add(tabPage_upload_tb_path);
            tabPage_upload.Controls.Add(tabPage_upload_btn_changeFile);
            tabPage_upload.Controls.Add(tabPage_upload_btn_save);
            tabPage_upload.Controls.Add(tabPage_upload_pb_image);
            tabPage_upload.Controls.Add(tabPage_upload_lbl_type_v);
            tabPage_upload.Controls.Add(tabPage_upload_lbl_type);
            tabPage_upload.Controls.Add(tabPage_upload_vtb_description);
            tabPage_upload.Controls.Add(tabPage_upload_lbl_description);
            tabPage_upload.Location = new Point(4, 24);
            tabPage_upload.Name = "tabPage_upload";
            tabPage_upload.Padding = new System.Windows.Forms.Padding(3);
            tabPage_upload.Size = new Size(2032, 1077);
            tabPage_upload.TabIndex = 0;
            tabPage_upload.Text = "Upload";
            tabPage_upload.UseVisualStyleBackColor = true;
            // 
            // tabPage_upload_lbl_extension_v
            // 
            tabPage_upload_lbl_extension_v.AutoSize = true;
            tabPage_upload_lbl_extension_v.Location = new Point(120, 69);
            tabPage_upload_lbl_extension_v.Name = "tabPage_upload_lbl_extension_v";
            tabPage_upload_lbl_extension_v.Size = new Size(70, 15);
            tabPage_upload_lbl_extension_v.TabIndex = 9;
            tabPage_upload_lbl_extension_v.Text = "--value--";
            // 
            // tabPage_upload_lbl_extension
            // 
            tabPage_upload_lbl_extension.AutoSize = true;
            tabPage_upload_lbl_extension.Location = new Point(23, 69);
            tabPage_upload_lbl_extension.Name = "tabPage_upload_lbl_extension";
            tabPage_upload_lbl_extension.Size = new Size(77, 15);
            tabPage_upload_lbl_extension.TabIndex = 8;
            tabPage_upload_lbl_extension.Text = "Extension:";
            // 
            // tabPage_upload_tb_path
            // 
            tabPage_upload_tb_path.Location = new Point(23, 383);
            tabPage_upload_tb_path.Name = "tabPage_upload_tb_path";
            tabPage_upload_tb_path.Size = new Size(320, 21);
            tabPage_upload_tb_path.TabIndex = 7;
            tabPage_upload_tb_path.Visible = false;
            // 
            // tabPage_upload_btn_changeFile
            // 
            tabPage_upload_btn_changeFile.Location = new Point(193, 97);
            tabPage_upload_btn_changeFile.Name = "tabPage_upload_btn_changeFile";
            tabPage_upload_btn_changeFile.Size = new Size(150, 35);
            tabPage_upload_btn_changeFile.TabIndex = 6;
            tabPage_upload_btn_changeFile.Text = "Change File";
            tabPage_upload_btn_changeFile.UseVisualStyleBackColor = true;
            tabPage_upload_btn_changeFile.Click += TabPage_upload_btn_changeFile_Click;
            // 
            // tabPage_upload_btn_save
            // 
            tabPage_upload_btn_save.Location = new Point(23, 97);
            tabPage_upload_btn_save.Name = "tabPage_upload_btn_save";
            tabPage_upload_btn_save.Size = new Size(150, 35);
            tabPage_upload_btn_save.TabIndex = 5;
            tabPage_upload_btn_save.Text = "Save";
            tabPage_upload_btn_save.UseVisualStyleBackColor = true;
            tabPage_upload_btn_save.Click += TabPage_upload_btn_save_Click;
            // 
            // tabPage_upload_pb_image
            // 
            tabPage_upload_pb_image.Location = new Point(23, 137);
            tabPage_upload_pb_image.Name = "tabPage_upload_pb_image";
            tabPage_upload_pb_image.Size = new Size(320, 240);
            tabPage_upload_pb_image.TabIndex = 4;
            tabPage_upload_pb_image.TabStop = false;
            tabPage_upload_pb_image.Visible = false;
            // 
            // tabPage_upload_lbl_type_v
            // 
            tabPage_upload_lbl_type_v.AutoSize = true;
            tabPage_upload_lbl_type_v.Location = new Point(120, 54);
            tabPage_upload_lbl_type_v.Name = "tabPage_upload_lbl_type_v";
            tabPage_upload_lbl_type_v.Size = new Size(70, 15);
            tabPage_upload_lbl_type_v.TabIndex = 3;
            tabPage_upload_lbl_type_v.Text = "--value--";
            // 
            // tabPage_upload_lbl_type
            // 
            tabPage_upload_lbl_type.AutoSize = true;
            tabPage_upload_lbl_type.Location = new Point(23, 54);
            tabPage_upload_lbl_type.Name = "tabPage_upload_lbl_type";
            tabPage_upload_lbl_type.Size = new Size(42, 15);
            tabPage_upload_lbl_type.TabIndex = 2;
            tabPage_upload_lbl_type.Text = "Type:";
            // 
            // tabPage_upload_vtb_description
            // 
            tabPage_upload_vtb_description.IsRequired = true;
            tabPage_upload_vtb_description.Location = new Point(120, 25);
            tabPage_upload_vtb_description.Name = "tabPage_upload_vtb_description";
            tabPage_upload_vtb_description.Size = new Size(300, 23);
            tabPage_upload_vtb_description.TabIndex = 1;
            // 
            // tabPage_upload_lbl_description
            // 
            tabPage_upload_lbl_description.AutoSize = true;
            tabPage_upload_lbl_description.Location = new Point(23, 33);
            tabPage_upload_lbl_description.Name = "tabPage_upload_lbl_description";
            tabPage_upload_lbl_description.Size = new Size(91, 15);
            tabPage_upload_lbl_description.TabIndex = 0;
            tabPage_upload_lbl_description.Text = "Description:";
            // 
            // tabPage_list
            // 
            tabPage_list.Location = new Point(4, 24);
            tabPage_list.Name = "tabPage_list";
            tabPage_list.Size = new Size(2032, 1077);
            tabPage_list.TabIndex = 1;
            tabPage_list.Text = "List";
            tabPage_list.UseVisualStyleBackColor = true;
            // 
            // tabPage_delete
            // 
            tabPage_delete.Controls.Add(tabPage_delete_tb_id);
            tabPage_delete.Controls.Add(tabPage_delete_btn_no);
            tabPage_delete.Controls.Add(tabPage_delete_btn_yes);
            tabPage_delete.Controls.Add(tabPage_delete_lbl_message);
            tabPage_delete.Location = new Point(4, 24);
            tabPage_delete.Name = "tabPage_delete";
            tabPage_delete.Size = new Size(2032, 1077);
            tabPage_delete.TabIndex = 2;
            tabPage_delete.Text = "Delete";
            tabPage_delete.UseVisualStyleBackColor = true;
            // 
            // tabPage_delete_tb_id
            // 
            tabPage_delete_tb_id.Location = new Point(8, 77);
            tabPage_delete_tb_id.Name = "tabPage_delete_tb_id";
            tabPage_delete_tb_id.Size = new Size(150, 21);
            tabPage_delete_tb_id.TabIndex = 7;
            tabPage_delete_tb_id.Visible = false;
            // 
            // tabPage_delete_btn_no
            // 
            tabPage_delete_btn_no.Location = new Point(164, 37);
            tabPage_delete_btn_no.Name = "tabPage_delete_btn_no";
            tabPage_delete_btn_no.Size = new Size(150, 35);
            tabPage_delete_btn_no.TabIndex = 6;
            tabPage_delete_btn_no.Text = "No";
            tabPage_delete_btn_no.UseVisualStyleBackColor = true;
            tabPage_delete_btn_no.Click += TabPage_delete_btn_no_Click;
            // 
            // tabPage_delete_btn_yes
            // 
            tabPage_delete_btn_yes.Location = new Point(8, 37);
            tabPage_delete_btn_yes.Name = "tabPage_delete_btn_yes";
            tabPage_delete_btn_yes.Size = new Size(150, 35);
            tabPage_delete_btn_yes.TabIndex = 5;
            tabPage_delete_btn_yes.Text = "Yes";
            tabPage_delete_btn_yes.UseVisualStyleBackColor = true;
            tabPage_delete_btn_yes.Click += TabPage_delete_btn_yes_Click;
            // 
            // tabPage_delete_lbl_message
            // 
            tabPage_delete_lbl_message.AutoSize = true;
            tabPage_delete_lbl_message.Location = new Point(8, 14);
            tabPage_delete_lbl_message.Name = "tabPage_delete_lbl_message";
            tabPage_delete_lbl_message.Size = new Size(266, 15);
            tabPage_delete_lbl_message.TabIndex = 4;
            tabPage_delete_lbl_message.Text = "Are you sure you want to delete \"{0}\"";
            // 
            // Content
            // 
            ClientSize = new Size(2040, 1155);
            Controls.Add(tabControl);
            Name = "Content";
            Controls.SetChildIndex(tabControl, 0);
            tabControl.ResumeLayout(false);
            tabPage_upload.ResumeLayout(false);
            tabPage_upload.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tabPage_upload_pb_image).EndInit();
            tabPage_delete.ResumeLayout(false);
            tabPage_delete.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdUpload;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_upload;
        private System.Windows.Forms.Label tabPage_upload_lbl_type_v;
        private System.Windows.Forms.Label tabPage_upload_lbl_type;
        private UT.Data.Controls.Validated.ValidatedTextBox tabPage_upload_vtb_description;
        private System.Windows.Forms.Label tabPage_upload_lbl_description;
        private System.Windows.Forms.PictureBox tabPage_upload_pb_image;
        private System.Windows.Forms.Button tabPage_upload_btn_save;
        private System.Windows.Forms.Button tabPage_upload_btn_changeFile;
        private System.Windows.Forms.TextBox tabPage_upload_tb_path;
        private System.Windows.Forms.TabPage tabPage_list;
        private System.Windows.Forms.Label tabPage_upload_lbl_extension_v;
        private System.Windows.Forms.Label tabPage_upload_lbl_extension;
        private System.Windows.Forms.TabPage tabPage_delete;
        private System.Windows.Forms.TextBox tabPage_delete_tb_id;
        private System.Windows.Forms.Button tabPage_delete_btn_no;
        private System.Windows.Forms.Button tabPage_delete_btn_yes;
        private System.Windows.Forms.Label tabPage_delete_lbl_message;
    }
}
