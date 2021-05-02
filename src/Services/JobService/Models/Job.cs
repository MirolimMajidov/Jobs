using Jobs.SharedModel.Helpers;
using Jobs.SharedModel.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace JobService.Models
{
    public class Job : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [DisplayName("Cost by $")]
        public int Cost { get; set; }

        public JobType Type { get; set; }

        public JobDuration Duration { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
    }
}
