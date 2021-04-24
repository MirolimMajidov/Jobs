using JobService.Models;
using JobService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : BaseController<Category>
    {
        public CategoryController(IEntityRepository<Category> pepository) : base(pepository)
        {
        }
    }
}
