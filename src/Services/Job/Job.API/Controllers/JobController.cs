using JobService.Models;
using Microsoft.AspNetCore.Mvc;
using Jobs.Service.Common.Controllers;
using Jobs.Service.Common.Helpers;
using Jobs.Service.Common.Repository;
using System.Threading.Tasks;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class JobController : BaseController<Job>
    {
        public JobController(IEntityRepository<Job> pepository) : base(pepository) { }

        public override async Task<RequestModel> Post([FromBody] Job entity)
        {
            entity.UserId = User.GetUserId();

            return await base.Post(entity);
        }
    }
}

