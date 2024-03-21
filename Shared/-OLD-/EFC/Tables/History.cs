using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.EFC.Tables
{
    [Table("Shared.History")]
    public class History
    {
        #region Enums
        public enum States
        {
            Updated, Deleted
        }
        #endregion //Enums

        #region Properties
        #region Fields
        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public string? Table { get; set; }
        [Required]
        public virtual Log? Log { get; set; }
        [Required]
        public byte[]? Data { get; set; }
        [Required]
        public States? State { get; set; }
        [Required]
        public DateTime TransStartDate { get; set; }
        #endregion //Fields
        #endregion //Properties
    }
}
