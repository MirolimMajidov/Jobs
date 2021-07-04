using Jobs.Service.Common;

namespace JobService.Models
{
    public class CategoryDTO : BaseEntityDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
