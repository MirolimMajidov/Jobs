using System;

namespace Jobs.Common.Models
{
    /// <summary>
    /// Root entity of all DTO entities
    /// </summary>
    public abstract class BaseEntityDTO : Disposable, IEntity
    {
        public Guid Id { get; set; }
    }
}
