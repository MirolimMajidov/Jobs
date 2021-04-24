using JobService.Models;
using System.Collections.Generic;

namespace JobService.Repository
{
    public interface IJobRepository
    {
        IEnumerable<Job> GetJobs();
        Job GetJobByID(int job);
        void InsertJob(Job job);
        void DeleteJob(int jobId);
        void UpdateJob(Job job);
        void Save();
    }
}
