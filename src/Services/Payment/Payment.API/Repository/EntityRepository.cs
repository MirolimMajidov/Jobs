using Jobs.Service.Common;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Repository
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : IEntity
    {
        private readonly IMongoCollection<TEntity> _entities;

        public EntityRepository(IMongoCollection<TEntity> entities)
        {
            _entities = entities;
        }

        /// <summary/>
        public async Task<List<TEntity>> GetEntities()
        {
            return await _entities.Find(c => true).ToListAsync();
        }

        /// <summary/>
        public IQueryable<TEntity> GetQueryableEntities()
        {
            throw new NotSupportedException("The MongoDB will not support IQueryable");
        }

        /// <summary/>
        public async Task<TEntity> GetEntityByID(Guid entityId)
        {
            return await _entities.Find(c => c.Id == entityId).FirstOrDefaultAsync();
        }

        /// <summary/>
        public async Task<TEntity> InsertEntity(TEntity entity, bool autoSave = true)
        {
            await _entities.InsertOneAsync(entity);
            return entity;
        }

        /// <summary/>
        public async Task UpdateEntity(TEntity entity, bool autoSave = true)
        {
            await _entities.ReplaceOneAsync(c => c.Id == entity.Id, entity);
        }

        /// <summary/>
        public async Task DeleteEntity(Guid entityId, bool autoSave = true)
        {
            await _entities.DeleteOneAsync(c => c.Id == entityId);
        }

        /// <summary/>
        public async Task Save()
        {
            throw await Task.FromResult(new NotSupportedException("The MongoDB will not support Save context"));
        }
    }
}
