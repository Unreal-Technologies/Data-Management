using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UT.Data.Efc;

namespace Shared.Efc.Tables
{
    [Table("Shared.Person")]
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
            return Firstname + " " + Lastname;
        }
        #endregion //public Methods
        #endregion //Properties
    }
}
