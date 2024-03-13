using Shared.Controls;
using Shared.EFC;
using Shared.EFC.Tables;
using System.Windows.Forms;
using UT.Data.Attributes;
using UT.Data.Controls;

namespace Shared.Modules
{
    [Position(int.MinValue, [ typeof(Main) ])]
    public class Roles : MdiGridviewFormModlet<Roles, Main, Context, Roles.Actions>
    {
        #region Enums
        public enum Actions
        {
            List
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
        #endregion //Implementations

        #region Private Methods
        public byte[]? Roles_DataReceived(Actions action, byte[] stream)
        {
            switch (action)
            {
                case Actions.List:
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
            return null;
        }

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
            this.ResumeLayout(false);
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
                            access.Add(Strings.Get(tier.ToString()));
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
        #endregion //Private Methods
    }
}
