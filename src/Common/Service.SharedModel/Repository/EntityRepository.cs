using Jobs.SharedModel.Models;
using Microsoft.EntityFrameworkCore;
using Service.SharedModel.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.SharedModel.Repository
{
    public class EntityRepository<TContext, TEntity> : IEntityRepository<TContext, TEntity> where TContext : DbContext where TEntity : BaseEntity
    {
        private readonly TContext _dbContext;

        public EntityRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary/>
        public async Task<IEnumerable<TEntity>> GetEntities()
        {
            return await _dbContext.GetEntities<TEntity>().ToListAsync();
        }

        /// <summary/>
        public async Task<TEntity> GetEntityByID(Guid entityId)
        {
            return await _dbContext.FindAsync(typeof(TEntity), entityId) as TEntity;
        }

        /// <summary/>
        public async Task<TEntity> InsertEntity(TEntity entity)
        {
            entity.Id = Guid.NewGuid();
            _dbContext.Add(entity);
            await Save();

            return entity;
        }

        /// <summary/>
        public async Task UpdateEntity(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await Save();
        }

        /// <summary/>
        public async Task DeleteEntity(Guid entityId)
        {
            var entity = _dbContext.Find(typeof(TEntity), entityId);
            _dbContext.Remove(entity);
            await Save();
        }

        /// <summary/>
        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
