using Shared.Controls;
using System.Windows.Forms;

namespace Shared.Modules.RolesModule
{
    public class RolesForm : CustomForm
    {
        #region Members
        private Gridview<Guid>? gridview;
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
            this.Load += RolesForm_Load;
        }

        private void RolesForm_Load(object? sender, EventArgs e)
        {
            this.SuspendLayout();

            List<Gridview<Guid>.Row> rows = [];
            for (int i = 0; i < 5; i++) //Test loop opbouw control
            {
                Gridview<Guid>.Row row = new()
                {
                    ID = Guid.NewGuid()
                };
                row.Cells.Add(new Gridview<Guid>.Cell()
                {
                    Text = "Desc " + i.ToString()
                });
                row.Cells.Add(new Gridview<Guid>.Cell()
                {
                    Text = "Acc " + i.ToString()
                });
                row.Cells.Add(new Gridview<Guid>.Cell()
                {
                    Text = "C " + i.ToString()
                });
                rows.Add(row);

                row.OnEdit += this.RowEdit;
                row.OnRemove += this.RowRemove;
            }
            this.gridview?.SetRows([.. rows]);

            this.ResumeLayout(false);
        }
        #endregion //Constructors

        public void RowEdit(Guid? id)
        {
            MessageBox.Show("EDIT: " + id);
        }

        public void RowRemove(Guid? id)
        {
            MessageBox.Show("REMOVE: " + id);
        }

        private void InitializeComponent()
        {
            gridview = new Gridview<Guid>();
            SuspendLayout();
            // 
            // gridview
            // 
            gridview.ControlLocation = Gridview<Guid>.ControlLocations.Left;
            gridview.Location = new System.Drawing.Point(0, 0);
            gridview.MinimumSize = new System.Drawing.Size(20, 20);
            gridview.Name = "gridview";
            gridview.Size = new System.Drawing.Size(544, 370);
            gridview.Font = this.Font;
            gridview.TabIndex = 0;
            gridview.SetColumns([new Gridview<Guid>.Column() { Text = "Description" }, new Gridview<Guid>.Column() { Text = "Access" }, new Gridview<Guid>.Column() { Text = "User Count" }]);
            // 
            // RolesForm
            // 
            ClientSize = new System.Drawing.Size(544, 448);
            Controls.Add(gridview);
            Name = "RolesForm";
            ResumeLayout(false);
        }
    }
}
