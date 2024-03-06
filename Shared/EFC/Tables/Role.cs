using System.ComponentModel.DataAnnotations;

namespace Shared.EFC.Tables
{
    public class Role
    {
        #region Enums
        public enum AccessTiers
        {
            Administrator, User
        }
        #endregion //Enums

        #region Properties
        #region Fields
        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public AccessTiers[]? Access { get; set; }
        [Required, MaxLength(64)]
        public string? Description { get; set; }
        [Required]
        public DateTime TransStartDate { get; set; }
        #endregion //Fields

        #region Collections
        public virtual ICollection<UserRole>? UserRoles { get; set; }
        #endregion //Collections
        #endregion //Properties
    }
}
