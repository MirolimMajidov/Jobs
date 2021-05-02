using AccountService.Models;
using Microsoft.AspNetCore.Mvc;
using Service.SharedModel.Controllers;
using Service.SharedModel.Helpers;
using Service.SharedModel.Repository;
using System.Threading.Tasks;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController<User>
    {
        public UserController(IEntityRepository<User> pepository) : base(pepository) { }

        public override async Task<RequestModel> Post([FromBody] User entity)
        {
            entity.HashPassword = Encryptor.SH1Hash(entity.Password);

            return await base.Post(entity);
        }
    }
}

