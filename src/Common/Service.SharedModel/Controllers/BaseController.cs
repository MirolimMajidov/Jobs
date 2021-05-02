﻿using Jobs.SharedModel.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.SharedModel.Repository;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace Service.SharedModel.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class BaseController<TEntity> : ControllerBase where TEntity : IEntity
    {
        private readonly IEntityRepository<TEntity> _pepository;

        public BaseController(IEntityRepository<TEntity> pepository)
        {
            _pepository = pepository;
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(Summary = "To get all items")]
        [SwaggerResponse(200, "Return list of found items if it's finished successfully", typeof(OkResult))]
        public virtual async Task<IActionResult> Get()
        {
            var entities = await _pepository.GetEntities();
            return new OkObjectResult(entities);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "To get an item by Id")]
        [SwaggerResponse(200, "Return the found item if it's finished successfully", typeof(OkResult))]
        [SwaggerResponse(404, "An item with the specified ID was not found", typeof(NotFoundResult))]
        public virtual async Task<IActionResult> Get(Guid id)
        {
            var entity = await _pepository.GetEntityByID(id);
            if (entity == null)
                return new NotFoundResult();

            return new OkObjectResult(entity);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "To add a new item. For this you must be authorized")]
        [SwaggerResponse(200, "Return OK if it's added successfully", typeof(OkResult))]
        public virtual async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            var createdEntity = await _pepository.InsertEntity(entity);
            return new OkObjectResult(createdEntity);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "To update exists an item. For this you must be authorized")]
        [SwaggerResponse(200, "Return OK if it's updated successfully", typeof(OkResult))]
        public virtual async Task<IActionResult> Put([FromBody] TEntity entity)
        {
            if (entity != null)
            {
                await _pepository.UpdateEntity(entity);
                return new OkResult();
            }
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "To delete an item. For this you must be authorized")]
        [SwaggerResponse(200, "Return OK if it's deleted successfully", typeof(OkResult))]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            await _pepository.DeleteEntity(id);
            return new OkResult();
        }
    }
}
