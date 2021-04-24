using Jobs.SharedModel.Models;
using JobService.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobService.Repository
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : IEntity
    {
        private readonly JobContext _dbContext;

        public EntityRepository(JobContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary/>
        public IEnumerable<TEntity> GetEntities()
        {
            return _dbContext.GetEntities<TEntity>().ToList();
        }

        /// <summary/>
        public TEntity GetEntityByID(Guid jobId)
        {
            return _dbContext.Find(typeof(TEntity), jobId) as TEntity;
        }

        /// <summary/>
        public void InsertEntity(TEntity entity)
        {
            entity.Id = Guid.NewGuid();
            _dbContext.Add(entity);
            Save();
        }

        /// <summary/>
        public void UpdateEntity(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            Save();
        }

        /// <summary/>
        public void DeleteEntity(Guid jobId)
        {
            var entity = _dbContext.Find(typeof(TEntity), jobId);
            _dbContext.Remove(entity);
            Save();
        }

        /// <summary/>
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
