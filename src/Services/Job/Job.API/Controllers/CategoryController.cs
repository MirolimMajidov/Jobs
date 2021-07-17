using AutoMapper;
using Jobs.Service.Common;
using JobService.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : BaseController<Category, CategoryDTO>
    {
        public CategoryController(IEntityQueryableRepository<Category> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
