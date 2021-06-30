using JobService.Models;
using Microsoft.AspNetCore.Mvc;
using Jobs.Service.Common.Controllers;
using Jobs.Service.Common.Helpers;
using Jobs.Service.Common.Repository;
using System.Threading.Tasks;
using AutoMapper;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class JobController : BaseController<Job, JobDTO>
    {
        public JobController(IEntityRepository<Job> repository, IMapper mapper) : base(repository, mapper) { }

        public override async Task<RequestModel> Post([FromBody] JobDTO entity)
        {
            entity.CreatedByUserId = User.GetUserId();
            entity.CreatedByUserName = User.GetUserName();

            return await base.Post(entity);
        }

        public override async Task<RequestModel> Put([FromBody] JobDTO entity)
        {
            entity.CreatedByUserId = User.GetUserId();
            entity.CreatedByUserName = User.GetUserName();

            return await base.Put(entity);
        }
    }
}

