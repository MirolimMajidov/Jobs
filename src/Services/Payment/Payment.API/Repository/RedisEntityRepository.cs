using Jobs.Service.Common;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Repository
{
    public class RedisEntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : IEntity
    {
        private readonly IDistributedCache _distributedCache;
        List<TEntity> _entities;

        public RedisEntityRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _entities = GetAllEntities();
        }

        List<TEntity> GetAllEntities()
        {
            var entitiesData = _distributedCache.Get(typeof(TEntity).Name);
            if (entitiesData == null)
                return new List<TEntity>();

            var serializedEntities = Encoding.UTF8.GetString(entitiesData);

            return JsonConvert.DeserializeObject<List<TEntity>>(serializedEntities);
        }

        /// <summary/>
        public async Task<List<TEntity>> GetEntities()
        {
            return await Task.FromResult(_entities);
        }

        /// <summary/>
        public async Task<TEntity> GetEntityByID(Guid entityId)
        {
            return await Task.FromResult(_entities.FirstOrDefault(c => c.Id == entityId));
        }

        /// <summary/>
        public async Task<TEntity> InsertEntity(TEntity entity, bool autoSave = true)
        {
            entity.Id = Guid.NewGuid();
            _entities.Add(entity);

            if (autoSave) await Save();

            return entity;
        }

        /// <summary/>
        public async Task UpdateEntity(TEntity entity, bool autoSave = true)
        {
            var index = _entities.IndexOf(entity);
            _entities[index] = entity;

            if (autoSave) await Save();
        }

        /// <summary/>
        public async Task DeleteEntity(Guid entityId, bool autoSave = true)
        {
            var entity = _entities.FirstOrDefault(c => c.Id == entityId);
            if (entity != null)
            {
                _entities.Remove(entity);

                if (autoSave) await Save();
            }
        }

        /// <summary/>
        public async Task Save()
        {
            var serializedEntities = JsonConvert.SerializeObject(_entities);
            var entitiesData = Encoding.UTF8.GetBytes(serializedEntities);
            await _distributedCache.SetAsync(typeof(TEntity).Name, entitiesData);
        }
    }
}
