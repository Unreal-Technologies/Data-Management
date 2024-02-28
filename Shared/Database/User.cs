using Shared.Modules;
using UT.Data.Attributes;
using UT.Data.DBE;
using UT.Data.DBE.Attributes;
using UT.Data.Encryption;

namespace Shared.Database
{
    [Description("DmCore.User")]
    public class User : Table<User, int>
    {
        #region Properties
        public string? Field_Username { get { return this.username; } set { this.username = value; } }
        public string? Field_Password { get { return this.password; } set { this.password = value; } }
        public Person? Object_Person { get { return Person.Single(this.personId); } set { this.personId = value?.GetPrimary(); } }
        public int? Field_PersonId { get { return this.personId; } set { this.personId = value; } }
        public int? Field_Id { get { return this.id; } set { this.id = value; } }
        public bool? Field_Enabled { get { return this.enabled; } set { this.enabled = value; } }
        #endregion //Properties

        #region Members
        private string? username;
        private string? password;
        private int? personId;
        [PrimaryKey]
        private int? id;
        private bool? enabled;
        #endregion //Members

        #region Public Methods
        public static User? Authenticate(UT.Data.DBE.IQueryable dbc, string username, string password)
        {
            string uName = Aes.Encrypt(username, DatabaseAccess.AuthenticationKey);
            string uPass = Aes.Encrypt(password, DatabaseAccess.AuthenticationKey);
            Query query = (new Query(dbc)).Select<User, int?>(x => x.Field_Id).From<User>().Where<User>(x => x.Field_Username == uName && x.Field_Password == uPass);

            var x = query.Execute();
            return null;
        }
        #endregion //Public Methods
    }
}
