﻿using Jobs.SharedModel.Models;
using MongoDB.Driver;
using Service.SharedModel.Repository;
using System;
using System.Collections.Generic;
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
        public async Task<IEnumerable<TEntity>> GetEntities()
        {
            return await _entities.Find(c => true).ToListAsync();
        }

        /// <summary/>
        public async Task<TEntity> GetEntityByID(Guid entityId)
        {
            return await _entities.Find(c => c.Id == entityId).FirstOrDefaultAsync();
        }

        /// <summary/>
        public async Task<TEntity> InsertEntity(TEntity entity)
        {
            await _entities.InsertOneAsync(entity);
            return entity;
        }

        /// <summary/>
        public async Task UpdateEntity(TEntity entity)
        {
            await _entities.ReplaceOneAsync(c => c.Id == entity.Id, entity);
        }

        /// <summary/>
        public async Task DeleteEntity(Guid entityId)
        {
            await _entities.DeleteOneAsync(c => c.Id == entityId);
        }
    }
}
