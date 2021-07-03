using JobService.Models;
using Microsoft.AspNetCore.Mvc;
using Jobs.Service.Common.Controllers;
using Jobs.Service.Common.Helpers;
using Jobs.Service.Common.Repository;
using System.Threading.Tasks;
using AutoMapper;
using Swashbuckle.AspNetCore.Annotations;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class JobController : BaseController<Job, JobDTO>
    {
        public JobController(IEntityRepository<Job> repository, IMapper mapper) : base(repository, mapper) { }

        public override async Task<RequestModel> Create([FromBody] JobDTO entity)
        {
            if (entity == null)
                return await base.Create(entity);

            entity.CreatedByUserId = User?.GetUserId();
            entity.CreatedByUserName = User?.GetUserName();

            return await base.Create(entity);
        }

        public override async Task<RequestModel> Update([FromBody] JobDTO entity)
        {
            if (entity == null)
                return await base.Update(entity);

            entity.CreatedByUserId = User?.GetUserId();
            entity.CreatedByUserName = User?.GetUserName();

            return await base.Update(entity);
        }

        [AllowAnonymous]
        [HttpGet("{categoryId}")]
        [SwaggerOperation(Summary = "To get job by category Id")]
        [SwaggerResponse(200, "Return the found items if it's finished successfully", typeof(RequestModel))]
        public virtual async Task<RequestModel> GetJobsByCategoryId(Guid categoryId)
        {
            var entities = (await _repository.GetEntities()).Where(e=>e.CategoryId == categoryId).Select(e => _mapper.Map<JobDTO>(e));
            return await RequestModel.SuccessAsync(entities);
        }
    }
}

