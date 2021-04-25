using JobService.DBContexts;
using JobService.Models;
using Microsoft.AspNetCore.Mvc;
using Service.SharedModel.Controllers;
using Service.SharedModel.Repository;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : BaseController<JobContext, Category>
    {
        public CategoryController(IEntityRepository<JobContext, Category> pepository) : base(pepository) { }
    }
}
