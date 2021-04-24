using JobService.Models;
using JobService.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Transactions;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly IEntityRepository<Category> _pepository;

        public CategoryController(IEntityRepository<Category> pepository)
        {
            _pepository = pepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var entities = _pepository.GetEntities();
            return new OkObjectResult(entities);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var entity = _pepository.GetEntityByID(id);
            return new OkObjectResult(entity);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Category entity)
        {
            using (var scope = new TransactionScope())
            {
                _pepository.InsertEntity(entity);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Category entity)
        {
            if (entity != null)
            {
                using (var scope = new TransactionScope())
                {
                    _pepository.UpdateEntity(entity);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _pepository.DeleteEntity(id);
            return new OkResult();
        }
    }
}
