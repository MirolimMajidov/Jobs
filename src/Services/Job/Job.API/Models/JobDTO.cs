using Jobs.Common.Helpers;
using Jobs.Common.Models;
using System;

namespace JobService.Models
{
    public class JobDTO : BaseEntityDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Cost { get; set; }

        public JobType Type { get; set; }

        public JobDuration Duration { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }

        public Guid CategoryId { get; set; }
    }
}
