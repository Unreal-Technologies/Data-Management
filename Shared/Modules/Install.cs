using Microsoft.EntityFrameworkCore;
using Shared.Efc;
using Shared.Efc.Tables;
using System.Net;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Efc;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(int.MinValue)]
    public class Install : IModlet
    {
        public void OnClientConfiguration(Form? form, ModletClient client, Session session) { }

        public void OnGlobalServerAction(byte[]? stream, IPAddress ip) { }

        public byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip) { return null; }

        public void OnServerConfiguration(ServerContext? context) { }

        public void OnServerInstallation(ServerContext? context)
        {
            if (context?.Select(this) is not SharedModContext smc || smc.People.Any())
            {
                return;
            }

            Person person = new()
            {
                Firstname = "Admin",
                Lastname = "Admin"
            };
            smc.Add(person);

            User user = new()
            {
                Username = "admin",
                Password = "admin",
                Person = person
            };
            smc.Add(user);

            Role role = new()
            {
                Description = "Administrator",
                Access = [Role.AccessTiers.Administrator, Role.AccessTiers.User],
            };
            smc.Add(role);

            UserRole userRole = new()
            {
                Role = role,
                User = user
            };
            smc.Add(userRole);
            smc.SaveChanges();
        }
    }
}
