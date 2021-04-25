using AccountService.DBContexts;
using AccountService.Models;
using Microsoft.AspNetCore.Mvc;
using Service.SharedModel.Controllers;
using Service.SharedModel.Repository;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController<AccountContext, User>
    {
        public UserController(IEntityRepository<AccountContext, User> pepository) : base(pepository) { }
    }
}

