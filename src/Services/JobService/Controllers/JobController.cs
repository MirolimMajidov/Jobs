using JobService.Models;
using Microsoft.AspNetCore.Mvc;
using Service.SharedModel.Controllers;
using Service.SharedModel.Repository;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class JobController : BaseController<Job>
    {
        public JobController(IEntityRepository<Job> pepository) : base(pepository) { }
    }
}

