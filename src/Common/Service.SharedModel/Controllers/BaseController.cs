using Jobs.SharedModel.Models;
using Service.SharedModel.Repository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Transactions;

namespace Service.SharedModel.Controllers
{
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    public abstract class BaseController<TEntity> : Controller where TEntity : IEntity
    {
        private readonly IEntityRepository<TEntity> _pepository;

        public BaseController(IEntityRepository<TEntity> pepository)
        {
            _pepository = pepository;
        }

        [SwaggerOperation(Summary = "To get all the items")]
        [HttpGet]
        public virtual IActionResult Get()
        {
            var entities = _pepository.GetEntities();
            return new OkObjectResult(entities);
        }

        [SwaggerOperation(Summary = "To get an item by Id")]
        [HttpGet("{id}")]
        public virtual IActionResult Get(Guid id)
        {
            var entity = _pepository.GetEntityByID(id);
            return new OkObjectResult(entity);
        }

        [SwaggerOperation(Summary = "To create a new item")]
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

        [SwaggerOperation(Summary = "To update exists an item")]
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

        [SwaggerOperation(Summary = "To delete an item")]
        [HttpDelete("{id}")]
        public virtual IActionResult Delete(Guid id)
        {
            _pepository.DeleteEntity(id);
            return new OkResult();
        }
    }
}
