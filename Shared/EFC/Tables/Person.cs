using System.ComponentModel.DataAnnotations;

namespace Shared.EFC.Tables
{
    public class Person
    {
        #region Properties
        #region Fields
        [Required, Key]
        public Guid Id { get; set; }
        [Required, MaxLength(32)]
        public string? Firstname { get; set; }
        [Required, MaxLength(32)]
        public string? Lastname { get; set; }
        [Required]
        public DateTime TransStartDate { get; set; }
        #endregion //Fields

        #region Public Methods
        public string Name()
        {
            return this.Firstname + " " + this.Lastname;
        }
        #endregion //public Methods

        #region Collections
        public virtual ICollection<User>? Users { get; set; }
        #endregion //Collections
        #endregion //Properties
    }
}
