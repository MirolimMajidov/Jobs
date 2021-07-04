using System;

namespace Jobs.Service.Common
{
    /// <summary>
    /// An interface for all entities
    /// </summary>
    public interface IEntity : IDisposable
    {
        public Guid Id { get; set; }
    }
}
