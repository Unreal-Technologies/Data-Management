using System.ComponentModel.DataAnnotations;

namespace Shared.EFC.Tables
{
    public class UserRole
    {
        #region Properties
        #region Fields
        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public virtual User? User { get; set; }
        [Required]
        public virtual Role? Role { get; set; }
        [Required]
        public DateTime TransStartDate { get; set; }
        #endregion //Fields
        #endregion //Properties
    }
}
