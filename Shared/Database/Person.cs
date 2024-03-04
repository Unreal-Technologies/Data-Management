using UT.Data.Attributes;
using UT.Data.DBE;
using UT.Data.DBE.Attributes;
using DefaultAttribute = UT.Data.DBE.Attributes.DefaultAttribute;

namespace Shared.Database
{
    [Description("Shared-Person")]
    public class Person : Table<Person, int>
    {
        #region Properties
        public string? Field_Firstname { get { return this.firstname; } set { this.firstname = value; this.Changed.Add("firstname"); } }
        public string? Field_Lastname { get { return this.lastname; } set { this.lastname = value; this.Changed.Add("lastname"); } }
        public int? Field_Id { get { return this.id; } }
        public DateTime? Field_TransStartDate { get { return this.transStartDate; } }
        #endregion //Properties

        #region Members
        [PrimaryKey]
        private int? id;
        [Length(32)]
        private string? firstname;
        [Length(32)]
        private string? lastname;
        [Default("now()")]
        private DateTime? transStartDate;
        #endregion //Members
    }
}
