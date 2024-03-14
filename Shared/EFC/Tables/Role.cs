using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.EFC.Tables
{
    [Table("Shared.Role")]
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
        #endregion //Properties
    }
}
