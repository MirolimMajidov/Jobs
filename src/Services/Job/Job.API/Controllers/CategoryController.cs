using JobService.Models;
using Microsoft.AspNetCore.Mvc;
using Jobs.Service.Common.Controllers;
using Jobs.Service.Common.Repository;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : BaseController<Category>
    {
        public CategoryController(IEntityRepository<Category> pepository) : base(pepository) { }
    }
}
