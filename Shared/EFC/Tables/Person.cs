namespace Shared.EFC.Tables
{
    public class Person
    {
        #region Properties
        #region Fields
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime TransStartDate { get; set; }
        #endregion //Fields

        #region Collections
        public virtual ICollection<User> Users { get; set; }
        #endregion //Collections
        #endregion //Properties
    }
}
