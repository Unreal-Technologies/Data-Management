using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UT.Data.Efc;
using UT.Data.Encryption;
using UT.Data.Extensions;

namespace Shared.Efc.Tables
{
    [Table("Shared.User")]
    public class User
    {
        #region Constants
        public const string Key = "0x42df1a6f";
        #endregion //Constants

        #region Fields
        private string? username;
        private string? password;
        #endregion //Fields

        #region Properties
        #region Fields
        [Required, Key]
        public Guid Id { get; set; }
        [Required, MaxLength(64)]
        public string? Username { get { return Aes.Decrypt(username, User.Key); } set { username = Aes.Encrypt(value, User.Key); } }
        [Required, MaxLength(32)]
        public string? Password { get { return password; } set { password = Aes.Encrypt(value, User.Key)?.Md5(); } }
        [Required]
        public virtual Person? Person { get; set; }
        [Required, DataType(DataType.Date), Column(TypeName = "date")]
        public DateTime Start { get; set; }
        [Required, DataType(DataType.Date), Column(TypeName = "date")]
        public DateTime End { get; set; }
        [Required]
        public DateTime TransStartDate { get; set; }
        #endregion //Fields
        #endregion //Properties

        #region Constructors
        public User()
        {
            Start = DateTime.Today;
            End = DateTime.MaxValue;
        }
        #endregion //Constructors
    }
}
