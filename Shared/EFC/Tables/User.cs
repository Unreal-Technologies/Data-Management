namespace Shared.EFC.Tables
{
    public class User
    {
        #region Properties
        #region Fields
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public virtual Person Person { get; set; }
        public DateTime TransStartDate { get; set; }
        #endregion //Fields
        #endregion //Properties
    }
}
