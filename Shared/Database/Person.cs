using UT.Data.Attributes;
using UT.Data.DBE;
using UT.Data.DBE.Attributes;

namespace Shared.Database
{
    [Description("Shared-Person")]
    public class Person : Table<Person, int>
    {
        #region Properties
        public string? Field_Firstname { get { return this.firstname; } set { this.firstname = value; } }
        public string? Field_Lastname { get { return this.lastname; } set { this.lastname = value; } }
        public int Field_Id { get { return this.id; } set { this.id = value; } }
        public bool Field_Enabled { get { return this.enabled; } set { this.enabled = value; } }
        #endregion //Properties

        #region Members
        [PrimaryKey]
        private int id;
        private bool enabled;
        private string? firstname;
        private string? lastname;
        #endregion //Members
    }
}
