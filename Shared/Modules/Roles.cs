using Microsoft.EntityFrameworkCore;
using Shared.EFC;
using Shared.EFC.Tables;
using Shared.Modlet;
using Shared.Modules.RolesModule;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(10)]
    public class Roles : IMdiFormModlet
    {
        #region Members
        private Form? main;
        private Context? context;
        #endregion //members

        #region Implementations
        void IModlet.OnClientConfiguration(Form? main)
        {
            this.main = main;
        }

        void IModlet.OnGlobalServerAction(byte[]? stream) { }

        byte[]? IModlet.OnLocalServerAction(byte[]? stream)
        {
            return null;
        }

        void IMdiFormModlet.OnMenuCreation(MenuItem menu)
        {
            if(ApplicationState.Access != null && ApplicationState.Access.Contains(Role.AccessTiers.Administrator))
            {
                MenuItem? administrator = menu.Search("Administrator");
                administrator?.Add("Roles", MenuItem.Button(this.Menu_Roles_Click));
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
        private void Menu_Roles_Click(object? sender, EventArgs e)
        {
            if(this.main == null)
            {
                return;
            }
            RolesForm form = new(this.main)
            {
                Text = "Roles"
            };
            form.Show();
        }
        #endregion //Private Methods
    }
}
