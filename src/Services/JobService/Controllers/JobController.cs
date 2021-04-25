using JobService.DBContexts;
using JobService.Models;
using Microsoft.AspNetCore.Mvc;
using Service.SharedModel.Controllers;
using Service.SharedModel.Repository;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class JobController : BaseController<JobContext, Job>
    {
        public JobController(IEntityRepository<JobContext, Job> pepository) : base(pepository) { }
    }
}

