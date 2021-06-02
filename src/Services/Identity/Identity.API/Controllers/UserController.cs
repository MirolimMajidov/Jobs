using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;
using Service.SharedModel.Controllers;
using Service.SharedModel.Helpers;
using Service.SharedModel.Repository;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController<User>
    {
        public UserController(IEntityRepository<User> pepository) : base(pepository) { }

        [SwaggerResponse(200, "Return OK if it's added successfully", typeof(User))]
        public override async Task<RequestModel> Post([FromBody] User entity)
        {
            entity.HashPassword = Encryptor.SH1Hash(entity.Password);

            return await base.Post(entity);
        }
    }
}

