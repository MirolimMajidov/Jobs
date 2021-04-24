using Jobs.SharedModel.Helpers;
using Jobs.SharedModel.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobService.Models
{
    public class Job 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Description { get; set; }

        [DisplayName("Cost by $")]
        public int Cost { get; set; }

        public JobType Type { get; set; }

        public JobDuration Duration { get; set; }

        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
    }
}
