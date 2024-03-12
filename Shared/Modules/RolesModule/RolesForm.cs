using Shared.Controls;
using System.Drawing;
using System.Windows.Forms;
using UT.Data.Controls;

namespace Shared.Modules.RolesModule
{
    public class RolesForm : CustomForm
    {
        #region Members
        private GridviewGuid? gridview;
        private GroupBox? gridBox;
        #endregion //Members

        #region Constructors
        public RolesForm(Form parent) : this()
        {
            this.MdiParent = parent;
        }
        public RolesForm() : base() 
        {
            this.WindowState = FormWindowState.Maximized;
            this.InitializeComponent();
            this.Resize += RolesForm_Resize;
            this.Load += RolesForm_Load;
        }

        private void RolesForm_Resize(object? sender, EventArgs e)
        {
            if(this.gridBox == null)
            {
                return;
            }

            this.gridBox.Size = new Size(this.Width, 400);
        }

        private void RolesForm_Load(object? sender, EventArgs e)
        {
            this.SuspendLayout();
            if(this.gridview == null)
            {
                return;
            }
            this.gridview.OnAdd += this.OnAdd;
            this.gridview.OnEdit += this.OnEdit;
            this.gridview.OnRemove += this.OnRemove;
            this.gridview.SetColumns([new GridviewGuid.Column() { Text = "Description" }, new GridviewGuid.Column() { Text = "Access" }, new GridviewGuid.Column() { Text = "User Count" }]);

            List<GridviewGuid.Row> rows = [];
            for (int i = 0; i < 5; i++) //Test loop opbouw control
            {
                GridviewGuid.Row row = new()
                {
                    ID = Guid.NewGuid()
                };
                row.Cells.Add(new GridviewGuid.Cell()
                {
                    Text = "Desc " + i.ToString(),
                    TextAlignment = GridviewGuid.Alignment.Right,
                    Color = Color.BlueViolet
                });
                row.Cells.Add(new GridviewGuid.Cell()
                {
                    Text = "Acc " + i.ToString()
                });
                row.Cells.Add(new GridviewGuid.Cell()
                {
                    Text = "C " + i.ToString(),
                    TextAlignment = GridviewGuid.Alignment.Center
                });
                rows.Add(row);
            }
            this.gridview?.SetRows([.. rows]);

            this.ResumeLayout(false);
        }
        #endregion //Constructors

        #region Private Methods
        private void OnAdd(Guid? id)
        {
            MessageBox.Show("ADD");
        }

        private void OnEdit(Guid? id)
        {
            MessageBox.Show("EDIT: " + id);
        }

        private void OnRemove(Guid? id)
        {
            MessageBox.Show("REMOVE: " + id);
        }

        private void InitializeComponent()
        {
            gridview = new GridviewGuid();
            gridBox = new GroupBox();
            SuspendLayout();
            //
            // gridBox
            //
            gridBox.Size = new Size(100, 100);
            gridBox.Controls.Add(gridview);
            gridBox.Text = "Roles";
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
            // 
            // RolesForm
            // 
            ClientSize = new Size(544, 448);
            Controls.Add(gridBox);
            Name = "RolesForm";
            ResumeLayout(false);
        }
        #endregion //Private Methods
    }
}
