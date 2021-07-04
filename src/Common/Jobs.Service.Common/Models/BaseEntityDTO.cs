using System;

namespace Jobs.Service.Common
{
    /// <summary>
    /// Root entity of all DTO entities
    /// </summary>
    public abstract class BaseEntityDTO : Disposable, IEntity
    {
        public Guid Id { get; set; }
    }
}
