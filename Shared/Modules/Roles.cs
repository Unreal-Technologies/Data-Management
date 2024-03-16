using Shared.Controls;
using Shared.EFC;
using Shared.EFC.Tables;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using UT.Data.Attributes;
using UT.Data.Controls;
using UT.Data.IO;

namespace Shared.Modules
{
    [Position(int.MinValue, [ typeof(Main) ])]
    public class Roles : MdiGridviewFormModlet<Roles, Main, Context, Roles.Actions>
    {
        #region Members
        Label? lblIdDynamic;
        TextBox? tbDescriptionDynamic;
        CheckedComboBox? ccbAccessDynamic;
        #endregion //Members

        #region Enums
        public enum Actions
        {
            List, Single
        }
        #endregion //Enums

        #region Constructors
        public Roles() : base()
        {
            this.Load += Roles_Load;
            this.OnDataReceived += Roles_DataReceived;
        }
        #endregion //Constructors

        #region Implementations
        public override void OnMenuCreation(MenuItem menu)
        {
            if(ApplicationState.Access != null && ApplicationState.Access.Contains(Role.AccessTiers.Administrator))
            {
                MenuItem? administrator = menu.Search(Strings.Word_Administrator);
                administrator?.Add(Strings.Word_Roles, MenuItem.Button(delegate (object? sender, EventArgs e) { this.Show(this.Main); }));
            }
        }

        public byte[]? Roles_DataReceived(Actions action, byte[] stream)
        {
            return action switch
            {
                Actions.Single => this.ActionSingle(stream),
                Actions.List => this.ActionList(),
                _ => null,
            };
        }
        #endregion //Implementations


        #region Protected Methods
        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            this.SuspendLayout();
            this.Text = Strings.Word_Roles;
            if (this.Gridview != null)
            {
                this.Gridview.SetColumns([new GridviewGuid.Column() { Text = Strings.Word_Description }, new GridviewGuid.Column() { Text = Strings.Word_Access }, new GridviewGuid.Column() { Text = Strings.String_UserCount }]);
                this.Gridview.OnAdd += this.OnAdd;
                this.Gridview.OnEdit += this.OnEdit;
                this.Gridview.OnRemove += this.OnRemove;
            }

            Padding padding = new(5, 20, 5, 5);

            //Static
            Label lblIdStatic = new()
            {
                Text = Strings.Word_Id + ":",
                Location = new Point(padding.Left, padding.Top)
            };
            this.ControlBox?.Controls.Add(lblIdStatic);
            lblIdStatic.Size = lblIdStatic.PreferredSize;

            Label lblDescriptionStatic = new()
            {
                Text = Strings.Word_Description + ":",
                Location = new Point(padding.Left, lblIdStatic.Location.Y + lblIdStatic.Height + padding.Bottom)
            };
            this.ControlBox?.Controls.Add(lblDescriptionStatic);
            lblDescriptionStatic.Size = lblDescriptionStatic.PreferredSize;

            Label lblAccessStatic = new()
            {
                Text = Strings.Word_Access + ":",
                Location = new Point(padding.Left, lblDescriptionStatic.Location.Y + lblDescriptionStatic.Height + padding.Bottom)
            };
            this.ControlBox?.Controls.Add(lblAccessStatic);
            lblAccessStatic.Size = lblAccessStatic.PreferredSize;

            int staticWidth = (new int[] { lblIdStatic.Width, lblDescriptionStatic.Width, lblAccessStatic.Width }).Max();

            //Dynamic
            this.lblIdDynamic = new()
            {
                Location = new Point(lblIdStatic.Location.X + staticWidth + padding.Left, padding.Top),
                Text = Guid.Empty.ToString(),
                Enabled = false,
            };
            this.ControlBox?.Controls.Add(this.lblIdDynamic);
            this.lblIdDynamic.Size = this.lblIdDynamic.PreferredSize;

            this.tbDescriptionDynamic = new()
            {
                Location = new Point(lblDescriptionStatic.Location.X + staticWidth + padding.Left, (lblDescriptionStatic.Location.Y - 2)),
                Text = "",
                Enabled = false,
                Width = 600
            };
            this.ControlBox?.Controls.Add(this.tbDescriptionDynamic);

            this.ccbAccessDynamic = new()
            {
                Location = new Point(lblAccessStatic.Location.X + staticWidth + padding.Left, (lblAccessStatic.Location.Y - 2)),
                Enabled = false,
                Width = 600,
                MaxDropDownItems = 5,
                DisplayMember = "Name",
                ValueSeparator = ", "
            };
            foreach(Role.AccessTiers tier in Enum.GetValues(typeof(Role.AccessTiers)))
            {
                this.ccbAccessDynamic.Items?.Add(new CcbItem(Strings.GetValue("W!" + tier.ToString()), (int)tier));
            }
            this.ControlBox?.Controls.Add(this.ccbAccessDynamic);

            this.ResumeLayout(false);
        }
        #endregion //Protected Methods

        #region Private Methods
        private byte[]? ActionSingle(byte[] stream)
        {
            byte[]? guid = Serializer<byte[]>.Deserialize(stream);
            if (guid == null)
            {
                return null;
            }

            Guid id = new(guid);
            Role? role = this.Context?.Role.Where(x => x.Id == id).FirstOrDefault();
            if (role == null)
            {
                return null;
            }
            return this.Response(role);
        }

        private byte[] ActionList()
        {
            List<Tuple<Role, int>> list = [];
            Role[]? roles = this.Context?.Role.OrderBy(x => x.Description).ToArray();
            if (roles != null)
            {
                foreach (Role role in roles)
                {
                    if (this.Context == null)
                    {
                        continue;
                    }
                    list.Add(new Tuple<Role, int>(role, this.Context.UserRole.Where(x => x.Role != null && x.Role.Id == role.Id).Count()));
                }
            }

            return this.Response<Tuple<Role, int>[]>([.. list]);
        }

        private void Roles_Load(object? sender, EventArgs e)
        {
            this.SuspendLayout();
            Tuple<Role, int>[]? data = this.Request<object, Tuple<Role, int>[]>(0, Actions.List);
            if(data != null)
            {
                List<GridviewGuid.Row> rows = [];
                foreach (Tuple<Role, int> item in data)
                {
                    Role role = item.Item1;
                    int count = item.Item2;

                    List<string> access = [];
                    if (role.Access != null)
                    {
                        foreach (Role.AccessTiers tier in role.Access)
                        {
                            access.Add(Strings.GetValue("W!" + tier.ToString()));
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
                this.Gridview?.SetRows([.. rows]);
            }

            this.ResumeLayout(false);
        }

        private void OnAdd(Guid? id)
        {
            this.UpdateControls(id, true, false, false);
        }

        private void OnEdit(Guid? id)
        {
            this.UpdateControls(id, false, true, false);
        }

        private void OnRemove(Guid? id)
        {
            this.UpdateControls(id, false, false, true);
        }

        private void UpdateControls(Guid? id, bool isAdd, bool isEdit, bool isRemove)
        {
            if(this.lblIdDynamic == null || this.tbDescriptionDynamic == null || this.ccbAccessDynamic == null)
            {
                return;
            }
            bool enable = isAdd || isEdit || isRemove;
            for(int i=0; i<this.ccbAccessDynamic.Items?.Count; i++)
            {
                this.ccbAccessDynamic.SetItemCheckState(i, CheckState.Unchecked);
            }
            this.lblIdDynamic.Text = (id == null ? Guid.Empty : id).ToString();
            if (id != null)
            {
                Role? role = this.Request<byte[], Role>(id.Value.ToByteArray(), Actions.Single);
                if (role != null)
                {
                    this.tbDescriptionDynamic.Text = role.Description ?? string.Empty;
                    foreach (Role.AccessTiers tier in role.Access ?? [])
                    {
                        this.ccbAccessDynamic.SetItemCheckState((int)tier, CheckState.Checked);
                    }
                }
            }

            this.tbDescriptionDynamic.Enabled = enable;
            this.ccbAccessDynamic.Enabled = enable;
        }
        #endregion //Private Methods
    }
}
