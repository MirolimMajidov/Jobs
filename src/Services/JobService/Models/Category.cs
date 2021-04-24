using Newtonsoft.Json;
using Jobs.SharedModel.Models;
using System.Collections.Generic;

namespace JobService.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
