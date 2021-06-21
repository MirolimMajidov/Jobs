using Jobs.Common.Helpers;
using Jobs.Common.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace IdentityService.Models
{
    public class User : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Login { get; set; }

        [NotMapped]
        public string Password { get; set; }

        /// <summary>
        /// Hash of user password
        /// </summary>
        [JsonIgnore, MaxLength(40)]
        public string HashPassword { get; set; }

        public double Balance { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public GenderStatus Gender { get; set; } = GenderStatus.NotSelected;

        /// <summary>
        /// Hash of last token. It will be needed to check curret token is valide of not.
        /// </summary>
        [JsonIgnore, MaxLength(40)]
        public string Token { get; set; }

        /// <summary>
        /// This is to get new token
        /// </summary>
        [JsonIgnore, MaxLength(40)]
        public string RefreshToken { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        [JsonIgnore]
        public DateTime LastOnline { get; set; } = DateTime.Now;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        [DisplayName("Registration's date")]
        [JsonIgnore]
        public DateTime RegistrationDate { get; internal set; } = DateTime.Now;
    }
}
