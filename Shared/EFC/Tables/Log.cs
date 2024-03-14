using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Shared.EFC.Tables
{
    [Table("Shared.Log")]
    public class Log
    {
        #region Properties
        #region Fields
        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public virtual User? User { get; set; }
        [Required]
        public IPAddress? IP { get; set; }
        [Required]
        public Strings.Languages? Language { get; set; }
        [Required]
        public string? Text { get; set; }
        [Required]
        public DateTime TransStartDate { get; set; }
        #endregion //Fields
        #endregion //Properties
    }
}
