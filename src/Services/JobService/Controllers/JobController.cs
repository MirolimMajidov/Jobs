using JobService.Models;
using JobService.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class JobController : Controller
    {
        private readonly IJobRepository _jobRepository;

        public JobController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var jobs = _jobRepository.GetJobs();
            return new OkObjectResult(jobs);
        }

        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var job = _jobRepository.GetJobByID(id);
            return new OkObjectResult(job);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Job job)
        {
            using (var scope = new TransactionScope())
            {
                _jobRepository.InsertJob(job);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = job.Id }, job);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Job job)
        {
            if (job != null)
            {
                using (var scope = new TransactionScope())
                {
                    _jobRepository.UpdateJob(job);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _jobRepository.DeleteJob(id);
            return new OkResult();
        }
    }
}
