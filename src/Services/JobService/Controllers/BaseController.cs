using Jobs.SharedModel.Models;
using JobService.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Transactions;

namespace JobService.Controllers
{
    public abstract class BaseController<TEntity> : Controller where TEntity : BaseEntity
    {
        private readonly IEntityRepository<TEntity> _pepository;

        public BaseController(IEntityRepository<TEntity> pepository)
        {
            _pepository = pepository;
        }

        [HttpGet]
        public virtual IActionResult Get()
        {
            var entities = _pepository.GetEntities();
            return new OkObjectResult(entities);
        }

        [HttpGet("{id}")]
        public virtual IActionResult Get(Guid id)
        {
            var entity = _pepository.GetEntityByID(id);
            return new OkObjectResult(entity);
        }

        [HttpPost]
        public virtual IActionResult Post([FromBody] TEntity entity)
        {
            using (var scope = new TransactionScope())
            {
                _pepository.InsertEntity(entity);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
        }

        [HttpPut]
        public virtual IActionResult Put([FromBody] TEntity entity)
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
        public virtual IActionResult Delete(Guid id)
        {
            _pepository.DeleteEntity(id);
            return new OkResult();
        }
    }
}
