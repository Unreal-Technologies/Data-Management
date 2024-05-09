using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Efc.Tables
{
    [Table("Shared.Content")]
    public class Content
    {
        #region Enums
        public enum Types
        {
            Image, Undefined
        }
        #endregion //Enums

        #region Properties
        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public virtual User? User { get; set; }
        [Required]
        public string? Stream { get; set; }
        public Types Type { get; set; }
        [Required]
        public DateTime TransStartDate { get; set; }
        #endregion //Properties
    }
}
