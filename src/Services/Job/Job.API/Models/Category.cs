using Jobs.Service.Common;
using System.Collections.Generic;

namespace JobService.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
