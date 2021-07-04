using Jobs.Service.Common;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace JobService.Models
{
    public class Job : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Cost { get; set; }

        public JobType Type { get; set; }

        public JobDuration Duration { get; set; }

        [Required]
        public Guid CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }

        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
    }
}
