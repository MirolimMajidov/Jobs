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
            entity.CreatedByUserId = User.GetUserId();
            entity.CreatedByUserName = User.GetUserName();

            return await base.Post(entity);
        }

        public override async Task<RequestModel> Put([FromBody] Job entity)
        {
            entity.CreatedByUserId = User.GetUserId();
            entity.CreatedByUserName = User.GetUserName();

            return await base.Put(entity);
        }
    }
}

