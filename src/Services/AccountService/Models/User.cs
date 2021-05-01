using Jobs.SharedModel.Helpers;
using Jobs.SharedModel.Models;

namespace AccountService.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public UserRole Role { get; set; } = UserRole.User;
    }
}
