using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using UT.Data.Controls;

namespace Shared.Controls
{
    public abstract class MdiGridviewFormModlet<Tform, TmdiParent, Tcontext, Tenum> : MdiFormModlet<Tform, TmdiParent, Tcontext, Tenum>
        where Tform : CustomForm
        where TmdiParent : CustomForm
        where Tcontext : DbContext
        where Tenum : Enum
    {
        #region Members
        private GridviewGuid? gridview;
        private GroupBox? gridBox, controlBox;
        private string? previousAdditionText;
        #endregion //Members

        #region Properties
        protected GridviewGuid? Gridview { get { return this.gridview; } }

        [AllowNull]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                if (this.gridBox != null)
                {
                    this.gridBox.Text = value;
                }
                if(this.controlBox != null)
                {
                    this.controlBox.Text = value;
                }
            }
        }
        #endregion //Properties

        #region Constructors
        public MdiGridviewFormModlet() : base()
        {
            this.InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Resize += MdiGridviewFormModlet_Resize;
        }
        #endregion //Constructors

        #region Events
        private void MdiGridviewFormModlet_Resize(object? sender, EventArgs e)
        {
            if (this.gridBox == null || this.controlBox == null)
            {
                return;
            }

            float scale = 0.25f;

            this.gridBox.Size = new Size(this.Width, (int)(this.Height * scale * 3));
            this.gridBox.Location = new Point(0, 0);

            this.controlBox.Size = new Size(this.Width, (int)(this.Height * scale));
            this.controlBox.Location = new Point(0, this.gridBox.Height);
        }
        #endregion //Events

        #region Protected Methods
        protected virtual void InitializeComponent()
        {
            gridview = new GridviewGuid();
            gridBox = new GroupBox();
            controlBox = new GroupBox();
            SuspendLayout();
            //
            // gridBox
            //
            gridBox.Size = new Size(100, 100);
            gridBox.Controls.Add(gridview);
            // 
            // gridview
            // 
            gridview.ControlLocation = GridviewGuid.ControlLocations.Left;
            gridview.Location = new Point(10, 25);
            gridview.MinimumSize = new Size(20, 20);
            gridview.Name = "gridview";
            gridview.Size = new Size(544, 370);
            gridview.Font = this.Font;
            gridview.TabIndex = 0;
            gridview.OnAdd += this.OnAdd;
            gridview.OnEdit += this.OnEdit;
            gridview.OnRemove += this.OnRemove;
            //
            // controlBox
            //
            controlBox.Size = new Size(100, 100);
            // 
            // RolesForm
            // 
            Controls.Add(gridBox);
            Controls.Add(controlBox);
            ResumeLayout(false);
        }
        #endregion //Protected Methods

        #region Private Methods
        private void UpdateControlText(string? addition)
        {
            string seperator = " - ";
            if(this.previousAdditionText == addition || this.controlBox == null)
            {
                return;
            }
            string text = this.controlBox.Text;
            if (this.previousAdditionText != null)
            {
                text = text[..^(seperator + this.previousAdditionText).Length];
            }

            this.controlBox.Text = text + seperator + addition;
            this.previousAdditionText = addition;
        }

        private void OnAdd(Guid? id)
        {
            this.UpdateControlText("Add");
        }

        private void OnEdit(Guid? id)
        {
            this.UpdateControlText("Edit");
        }

        private void OnRemove(Guid? id)
        {
            this.UpdateControlText("Remove");
        }
        #endregion //Private Methods
    }
}
