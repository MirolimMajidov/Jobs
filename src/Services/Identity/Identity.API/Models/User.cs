using Jobs.Service.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public string Login { get; set; }

        /// <summary>
        /// Hash of user password
        /// </summary>
        [MaxLength(40)]
        public string HashPassword { get; set; }

        public double Balance { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public GenderStatus Gender { get; set; } = GenderStatus.NotSelected;

        /// <summary>
        /// Hash of last token. It will be needed to check curret token is valide of not.
        /// </summary>
        [MaxLength(40)]
        public string Token { get; set; }

        /// <summary>
        /// This is to get new token
        /// </summary>
        [MaxLength(40)]
        public string RefreshToken { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime LastOnline { get; set; } = DateTime.Now;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        [DisplayName("Registration's date")]
        public DateTime RegistrationDate { get; internal set; } = DateTime.Now;
    }
}
