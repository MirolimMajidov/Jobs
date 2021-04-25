using System;
using System.Collections.Generic;

namespace Service.SharedModel.Repository
{
    public interface IEntityRepository<TEntity>
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        IEnumerable<TEntity> GetEntities();

        /// <summary>
        /// Get entity by ID
        /// </summary>
        TEntity GetEntityByID(Guid entityId);

        /// <summary>
        /// To insert new entity to the DB
        /// </summary>
        void InsertEntity(TEntity entity);

        /// <summary>
        /// To update exists entity
        /// </summary>
        void UpdateEntity(TEntity entity);

        /// <summary>
        /// To delete entity
        /// </summary>
        void DeleteEntity(Guid entityId);

        /// <summary>
        /// To save changes of context
        /// </summary>
        void Save();
    }
}
