using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;
using Jobs.Service.Common.Controllers;
using Jobs.Service.Common.Helpers;
using Jobs.Service.Common.Repository;
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

