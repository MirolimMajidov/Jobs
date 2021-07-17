using System.Linq;

namespace Jobs.Service.Common
{
    public interface IEntityQueryableRepository<TEntity> : IEntityRepository<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Get all entities without ToList
        /// </summary>
        IQueryable<TEntity> GetQueryableEntities();
    }
}
