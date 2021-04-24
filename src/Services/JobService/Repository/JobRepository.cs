using Microsoft.EntityFrameworkCore;
using JobService.DBContexts;
using JobService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobService.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly JobContext _dbContext;

        public JobRepository(JobContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteJob(int jobId)
        {
            var product = _dbContext.Jobs.Find(jobId);
            _dbContext.Jobs.Remove(product);
            Save();
        }

        public Job GetJobByID(int jobId)
        {
            return _dbContext.Jobs.Find(jobId);
        }

        public IEnumerable<Job> GetJobs()
        {
            return _dbContext.Jobs.ToList();
        }

        public void InsertJob(Job job)
        {
            job.Id = Guid.NewGuid();
            _dbContext.Add(job);
            Save();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateJob(Job job)
        {
            _dbContext.Entry(job).State = EntityState.Modified;
            Save();
        }
    }
}
