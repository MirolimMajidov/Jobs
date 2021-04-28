using AccountService.Models;
using Microsoft.AspNetCore.Mvc;
using Service.SharedModel.Controllers;
using Service.SharedModel.Repository;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController<User>
    {
        public UserController(IEntityRepository<User> pepository) : base(pepository) { }
    }
}

