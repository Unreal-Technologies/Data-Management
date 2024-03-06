using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UT.Data.Encryption;

namespace Shared.EFC.Tables
{
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
        public string? Username { get { return Aes.Decrypt(this.username, User.Key); } set { this.username = Aes.Encrypt(value, User.Key); } }
        [Required, MaxLength(64)]
        public string? Password { get { return Aes.Decrypt(this.password, User.Key); } set { this.password = Aes.Encrypt(value, User.Key); } }
        [Required]
        public virtual Person? Person { get; set; }
        [Required, DataType(DataType.Date), Column(TypeName = "date")]
        public DateTime Start { get; set; }
        [Required, DataType(DataType.Date), Column(TypeName = "date")]
        public DateTime End { get; set; }
        [Required]
        public DateTime TransStartDate { get; set; }
        #endregion //Fields

        #region Collections
        public virtual ICollection<UserRole>? UserRoles { get; set; }
        #endregion //Collections
        #endregion //Properties

        #region Constructors
        public User()
        {
            this.Start = DateTime.Today;
            this.End = DateTime.MaxValue;
        }
        #endregion //Constructors
    }
}
