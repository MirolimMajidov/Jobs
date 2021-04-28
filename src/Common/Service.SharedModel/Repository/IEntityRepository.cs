using Jobs.SharedModel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.SharedModel.Repository
{
    public interface IEntityRepository<TContext, TEntity> where TContext : DbContext where TEntity : BaseEntity
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        Task<IEnumerable<TEntity>> GetEntities();

        /// <summary>
        /// Get entity by ID
        /// </summary>
        Task<TEntity> GetEntityByID(Guid entityId);

        /// <summary>
        /// To insert new entity to the DB
        /// </summary>
        Task<TEntity> InsertEntity(TEntity entity);

        /// <summary>
        /// To update exists entity
        /// </summary>
        Task UpdateEntity(TEntity entity);

        /// <summary>
        /// To delete entity
        /// </summary>
        Task DeleteEntity(Guid entityId);

        /// <summary>
        /// To save changes of context
        /// </summary>
        Task Save();
    }
}
