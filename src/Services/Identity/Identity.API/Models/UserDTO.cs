using Jobs.Common.Helpers;
using Jobs.Common.Models;

namespace IdentityService.Models
{
    public class UserDTO : BaseEntityDTO
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public double Balance { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public GenderStatus Gender { get; set; } = GenderStatus.NotSelected;
    }
}
