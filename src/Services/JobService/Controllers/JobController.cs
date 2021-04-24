using JobService.Models;
using JobService.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Transactions;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class JobController : Controller
    {
        private readonly IEntityRepository<Job> _jobRepository;

        public JobController(IEntityRepository<Job> jobRepository)
        {
            _jobRepository = jobRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var jobs = _jobRepository.GetEntities();
            return new OkObjectResult(jobs);
        }

        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(Guid id)
        {
            var job = _jobRepository.GetEntityByID(id);
            return new OkObjectResult(job);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Job job)
        {
            using (var scope = new TransactionScope())
            {
                _jobRepository.InsertEntity(job);
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
                    _jobRepository.UpdateEntity(job);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _jobRepository.DeleteEntity(id);
            return new OkResult();
        }
    }
}
