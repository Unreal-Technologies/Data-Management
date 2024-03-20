using Shared.Controls;
using Shared.EFC;
using Shared.EFC.Tables;
using System.Drawing;
using System.Windows.Forms;
using UT.Data.Attributes;
using UT.Data.Controls;
using UT.Data.Controls.Custom;
using UT.Data.IO;

namespace Shared.Modules
{
    [Position(int.MinValue, [ typeof(Main) ])]
    public class Roles : MdiGridviewFormModlet<Roles, Main, Context, Roles.Actions>
    {
        #region Members
        Label? lblIdDynamic;
        Validated<TextBox>? tbDescriptionDynamic;
        Validated<CheckedComboBox>? ccbAccessDynamic;
        Button? btnSave;
        #endregion //Members

        #region Enums
        public enum Actions
        {
            List, Single, InsertOrUpdate, Delete
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
                Actions.InsertOrUpdate => this.ActionInsertOrUpdate(stream),
                Actions.Delete => this.ActionDelete(stream),
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

            this.tbDescriptionDynamic = new Validated<TextBox>(delegate (TextBox control) { return control.Text; })
            {
                Location = new Point(lblDescriptionStatic.Location.X + staticWidth + padding.Left, (lblDescriptionStatic.Location.Y - 2)),
                Text = "",
                Enabled = false,
                Width = 600,
                IsRequired = true
            };
            this.tbDescriptionDynamic.Control.Width = 600;
            this.ControlBox?.Controls.Add(this.tbDescriptionDynamic);

            this.ccbAccessDynamic = new Validated<CheckedComboBox>(delegate (CheckedComboBox control) { return control.CheckedItems?.Count == 0 ? null : control.CheckedItems?.Count.ToString(); })
            {
                Location = new Point(lblAccessStatic.Location.X + staticWidth + padding.Left, (lblAccessStatic.Location.Y - 2)),
                Enabled = false,
                IsRequired = true
            };
            this.ccbAccessDynamic.Control.MaxDropDownItems = 5;
            this.ccbAccessDynamic.Control.DisplayMember = "Name";
            this.ccbAccessDynamic.Control.ValueSeparator = ", ";
            this.ccbAccessDynamic.Control.Width = 600;
            foreach (Role.AccessTiers tier in Enum.GetValues(typeof(Role.AccessTiers)))
            {
                this.ccbAccessDynamic.Control.Items?.Add(new CcbItem(Strings.GetValue("W!" + tier.ToString()), (int)tier));
            }
            this.ControlBox?.Controls.Add(this.ccbAccessDynamic);

            this.btnSave = new()
            {
                Enabled = false,
                Text = Strings.Word_Save,
                Width = 150,
                Height = 34
            };
            this.btnSave.Location = new Point(this.ccbAccessDynamic.Location.X + this.ccbAccessDynamic.Width - this.btnSave.Width, (this.ccbAccessDynamic.Location.Y + this.ccbAccessDynamic.Height - 2));
            this.btnSave.Click += BtnSave_Click;
            this.ControlBox?.Controls.Add(this.btnSave);

            this.ResumeLayout(false);
        }
        #endregion //Protected Methods

        #region Private Methods
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            Validator validator = new();
            validator.Add(this.ccbAccessDynamic);
            validator.Add(this.tbDescriptionDynamic);
            validator.Validate();
            if(!validator.IsValid || this.ccbAccessDynamic == null || this.tbDescriptionDynamic == null || this.lblIdDynamic == null)
            {
                return;
            }

            CheckedComboBox ccbAccess = this.ccbAccessDynamic.Control;
            TextBox tbDescription = this.tbDescriptionDynamic.Control;

            string description = tbDescription.Text;
            List<Role.AccessTiers> selected = [];
            if (ccbAccess.CheckedItems != null)
            {
                foreach (CcbItem item in ccbAccess.CheckedItems)
                {
                    selected.Add((Role.AccessTiers)item.Value);
                }
            }

            Role role = new()
            {
                Access = [.. selected],
                Description = description,
                Id = Guid.Parse(this.lblIdDynamic.Text)
            };
            bool isDelete = !this.tbDescriptionDynamic.Enabled;

            Tuple<bool>? result = this.Request<Role, Tuple<bool>>(role, isDelete ? Actions.Delete : Actions.InsertOrUpdate);
            if (result != null && result.Item1)
            {
                this.UpdateControls(null, false, false, false);
                this.Roles_Load(null, EventArgs.Empty);
            }
        }

        private byte[]? ActionDelete(byte[] stream)
        {
            Role? data = Serializer<Role>.Deserialize(stream);
            if (data != null && this.Context != null)
            {
                Role role = this.Context.Role.Where(x => x.Id == data.Id).First();
                this.Context.Role.Remove(role);
                this.Context.SaveChanges();
                return this.Response(new Tuple<bool>(true));
            }
            return null;
        }

        private byte[]? ActionInsertOrUpdate(byte[] stream)
        {
            Role? data = Serializer<Role>.Deserialize(stream);
            if(data == null || this.Context == null || this.tbDescriptionDynamic == null)
            {
                return null;
            }
            if(data.Id == Guid.Empty)
            {
                this.Context.Role.Add(data);
                this.Context.SaveChanges();
                return this.Response(new Tuple<bool>(true));
            }
            else
            {
                Role role = this.Context.Role.Where(x => x.Id == data.Id).First();
                role.Description = data.Description;
                role.Access = data.Access;

                this.Context.SaveChanges();
                return this.Response(new Tuple<bool>(true));
            }
        }

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
            if(this.lblIdDynamic == null || this.tbDescriptionDynamic == null || this.ccbAccessDynamic == null || this.btnSave == null)
            {
                return;
            }
            bool enable = isAdd || isEdit;
            for(int i=0; i<this.ccbAccessDynamic.Control.Items?.Count; i++)
            {
                this.ccbAccessDynamic.Control.SetItemCheckState(i, CheckState.Unchecked);
            }
            this.tbDescriptionDynamic.Control.Text = string.Empty;

            this.lblIdDynamic.Text = (id == null ? Guid.Empty : id).ToString();
            if (id != null)
            {
                Role? role = this.Request<byte[], Role>(id.Value.ToByteArray(), Actions.Single);
                if (role != null)
                {
                    this.tbDescriptionDynamic.Control.Text = role.Description ?? string.Empty;
                    foreach (Role.AccessTiers tier in role.Access ?? [])
                    {
                        this.ccbAccessDynamic.Control.SetItemCheckState((int)tier, CheckState.Checked);
                    }
                }
            }

            this.tbDescriptionDynamic.Enabled = enable;
            this.ccbAccessDynamic.Enabled = enable;
            this.btnSave.Enabled = enable || isRemove;
        }
        #endregion //Private Methods
    }
}
