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
            tabPage_upload_tb_path = new System.Windows.Forms.TextBox();
            tabPage_upload_btn_changeFile = new System.Windows.Forms.Button();
            tabPage_upload_btn_save = new System.Windows.Forms.Button();
            tabPage_upload_pb_image = new System.Windows.Forms.PictureBox();
            tabPage_upload_lbl_type_v = new System.Windows.Forms.Label();
            tabPage_upload_lbl_type = new System.Windows.Forms.Label();
            tabPage_upload_vtb_description = new UT.Data.Controls.Validated.ValidatedTextBox();
            tabPage_upload_lbl_description = new System.Windows.Forms.Label();
            tabPage_list = new System.Windows.Forms.TabPage();
            tabControl.SuspendLayout();
            tabPage_upload.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tabPage_upload_pb_image).BeginInit();
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
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.Location = new Point(0, 50);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(2040, 1105);
            tabControl.TabIndex = 1;
            // 
            // tabPage_upload
            // 
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
            // tabPage_upload_tb_path
            // 
            tabPage_upload_tb_path.Location = new Point(59, 358);
            tabPage_upload_tb_path.Name = "tabPage_upload_tb_path";
            tabPage_upload_tb_path.Size = new Size(320, 21);
            tabPage_upload_tb_path.TabIndex = 7;
            tabPage_upload_tb_path.Visible = false;
            // 
            // tabPage_upload_btn_changeFile
            // 
            tabPage_upload_btn_changeFile.Location = new Point(229, 72);
            tabPage_upload_btn_changeFile.Name = "tabPage_upload_btn_changeFile";
            tabPage_upload_btn_changeFile.Size = new Size(150, 35);
            tabPage_upload_btn_changeFile.TabIndex = 6;
            tabPage_upload_btn_changeFile.Text = "Change File";
            tabPage_upload_btn_changeFile.UseVisualStyleBackColor = true;
            tabPage_upload_btn_changeFile.Click += TabPage_upload_btn_changeFile_Click;
            // 
            // tabPage_upload_btn_save
            // 
            tabPage_upload_btn_save.Location = new Point(59, 72);
            tabPage_upload_btn_save.Name = "tabPage_upload_btn_save";
            tabPage_upload_btn_save.Size = new Size(150, 35);
            tabPage_upload_btn_save.TabIndex = 5;
            tabPage_upload_btn_save.Text = "Save";
            tabPage_upload_btn_save.UseVisualStyleBackColor = true;
            tabPage_upload_btn_save.Click += TabPage_upload_btn_save_Click;
            // 
            // tabPage_upload_pb_image
            // 
            tabPage_upload_pb_image.Location = new Point(59, 112);
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
            tabPage_upload_lbl_type.Location = new Point(72, 54);
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
            tabPage_upload_lbl_description.Location = new Point(23, 25);
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
            // ContentUpload
            // 
            ClientSize = new Size(2040, 1155);
            Controls.Add(tabControl);
            Name = "ContentUpload";
            Controls.SetChildIndex(tabControl, 0);
            tabControl.ResumeLayout(false);
            tabPage_upload.ResumeLayout(false);
            tabPage_upload.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tabPage_upload_pb_image).EndInit();
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
    }
}
