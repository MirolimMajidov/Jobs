using JobService.Models;
using Microsoft.AspNetCore.Mvc;
using Service.SharedModel.Controllers;
using Service.SharedModel.Helpers;
using Service.SharedModel.Repository;
using System.Threading.Tasks;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class JobController : BaseController<Job>
    {
        public JobController(IEntityRepository<Job> pepository) : base(pepository) { }

        public override async Task<IActionResult> Post([FromBody] Job entity)
        {
            entity.UserId = User.GetUserId();

            return await base.Post(entity);
        }
    }
}

