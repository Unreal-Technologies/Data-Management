using Microsoft.EntityFrameworkCore;
using Shared.Controls;
using Shared.EFC;
using Shared.EFC.Tables;
using Shared.Modlet;
using System.Drawing;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Controls;
using UT.Data.Modlet;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Shared.Modules
{
    [Position(int.MinValue, [ typeof(Main) ])]
    public class Roles : CustomForm<Roles>, IMdiFormModlet
    {
        #region Members
        private Form? main;
        private Context? context;
        private GridviewGuid? gridview;
        private GroupBox? gridBox;
        #endregion //Members

        #region Enums
        private enum Actions
        {
            List
        }
        #endregion //Enums

        #region Constructos
        public Roles() : base()
        {
            this.WindowState = FormWindowState.Maximized;
            this.InitializeComponent();
            this.Resize += Roles_Resize;
            this.Load += Roles_Load;
        }
        #endregion //Constructors

        #region Implementations
        void IModlet.OnClientConfiguration(Form? main)
        {
            this.main = main;
        }

        void IModlet.OnGlobalServerAction(byte[]? stream) { }

        byte[]? IModlet.OnLocalServerAction(byte[]? stream)
        {
            object? packet = Packet<Actions, object>.Decode(stream);
            if (packet == null)
            {
                return null;
            }

            switch (((Packet<Actions, object>)packet).Description)
            {
                case Actions.List:
                    List<Tuple<Role?, int?>> list = [];
                    Role[]? roles = this.context?.Role.OrderBy(x => x.Description).ToArray();
                    if(roles != null)
                    {
                        foreach(Role role in roles)
                        {
                            list.Add(new Tuple<Role?, int?>(role, this.context?.UserRole.Where(x => x.Role != null && x.Role.Id == role.Id).Count()));
                        }
                    }

                    return Packet<bool, Tuple<Role?, int?>[]>.Encode(true, [.. list]);
            }

            return null;
        }

        void IMdiFormModlet.OnMenuCreation(MenuItem menu)
        {
            if(ApplicationState.Access != null && ApplicationState.Access.Contains(Role.AccessTiers.Administrator))
            {
                MenuItem? administrator = menu.Search("Administrator");
                administrator?.Add("Roles", MenuItem.Button(delegate (object? sender, EventArgs e) { this.Show(this.main); }));
            }
        }

        void IModlet.OnSequentialExecutionConfiguration(SequentialExecution se) { }

        void IModlet.OnServerConfiguration(DbContext? context, ref Dictionary<string, object?> configuration)
        {
            this.context = context as Context;
        }

        void IModlet.OnServerInstallation(DbContext? context) { }
        #endregion //Implementations

        #region Private Methods
        private void Roles_Resize(object? sender, EventArgs e)
        {
            if (this.gridBox == null)
            {
                return;
            }

            this.gridBox.Size = new Size(this.Width, 400);
        }

        private void Roles_Load(object? sender, EventArgs e)
        {
            this.SuspendLayout();
            if (this.gridview == null)
            {
                return;
            }
            this.gridview.OnAdd += this.OnAdd;
            this.gridview.OnEdit += this.OnEdit;
            this.gridview.OnRemove += this.OnRemove;
            this.gridview.SetColumns([new GridviewGuid.Column() { Text = "Description" }, new GridviewGuid.Column() { Text = "Access" }, new GridviewGuid.Column() { Text = "User Count" }]);

            Tuple<Role?, int?>[]? data = Packet<bool, Tuple<Role?, int?>[]>.Decode(ApplicationState.Client?.Send(Packet<Actions, int>.Encode(Actions.List, 0), ModletCommands.Commands.Action, this))?.Data;
            if(data != null)
            {
                List<GridviewGuid.Row> rows = [];
                foreach (Tuple<Role?, int?> item in data)
                {
                    Role? role = item.Item1;
                    int? count = item.Item2;
                    if (role == null || count == null)
                    {
                        continue;
                    }

                    List<string> access = [];
                    if (role.Access != null)
                    {
                        foreach (Role.AccessTiers tier in role.Access)
                        {
                            access.Add(tier.ToString());
                        }
                    }

                    GridviewGuid.Row row = new()
                    {
                        ID = role.Id,
                        Remove = count <= 0
                    };
                    row.Cells.Add(new GridviewGuid.Cell()
                    {
                        Text = role.Description
                    });
                    row.Cells.Add(new GridviewGuid.Cell()
                    {
                        Text = string.Join(", ", access)
                    });
                    row.Cells.Add(new GridviewGuid.Cell()
                    {
                        Text = count.ToString(),
                        TextAlignment = GridviewGuid.Alignment.Right
                    });
                    rows.Add(row);
                }
                this.gridview?.SetRows([.. rows]);
            }

            this.ResumeLayout(false);
        }

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
            Name = "Roles";
            Text = "Roles";
            ResumeLayout(false);
        }
        #endregion //Private Methods
    }
}
