using IdentityService.DataProvider;
using Jobs.Common.Models;
using Jobs.Service.Common.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Repository
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly JobsContext _dbContext;

        public EntityRepository(JobsContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary/>
        public async Task<IEnumerable<TEntity>> GetEntities()
        {
            return await _dbContext.GetEntities<TEntity>().ToListAsync();
        }

        /// <summary/>
        public IQueryable<TEntity> GetQueryableEntities()
        {
            return _dbContext.GetEntities<TEntity>();
        }

        /// <summary/>
        public async Task<TEntity> GetEntityByID(Guid entityId)
        {
            return await _dbContext.FindAsync(typeof(TEntity), entityId) as TEntity;
        }

        /// <summary/>
        public async Task<TEntity> InsertEntity(TEntity entity, bool autoSave = true)
        {
            entity.Id = Guid.NewGuid();
            _dbContext.Add(entity);

            if (autoSave)
                await Save();

            return entity;
        }

        /// <summary/>
        public async Task UpdateEntity(TEntity entity, bool autoSave = true)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            if (autoSave)
                await Save();
        }

        /// <summary/>
        public async Task DeleteEntity(Guid entityId, bool autoSave = true)
        {
            var entity = _dbContext.Find(typeof(TEntity), entityId);
            _dbContext.Remove(entity);

            if (autoSave)
                await Save();
        }

        /// <summary/>
        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
