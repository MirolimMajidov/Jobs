using JobService.Models;
using JobService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class JobController : BaseController<Job>
    {
        public JobController(IEntityRepository<Job> pepository) : base(pepository)
        {
        }
    }
}
