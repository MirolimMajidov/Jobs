using JobService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.SharedModel.Controllers;
using Service.SharedModel.Repository;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : BaseController<Category>
    {
        public CategoryController(IEntityRepository<Category> pepository) : base(pepository) { }
    }
}
