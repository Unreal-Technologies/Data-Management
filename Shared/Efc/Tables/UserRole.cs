using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Efc.Tables
{
    [Table("Shared.User+Role")]
    public class UserRole
    {
        #region Properties
        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public virtual User? User { get; set; }
        [Required]
        public virtual Role? Role { get; set; }
        [Required]
        public DateTime TransStartDate { get; set; }
        #endregion //Properties
    }
}
