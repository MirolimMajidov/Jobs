using System;

namespace Jobs.SharedModel.Models
{
    /// <summary>
    /// An interface for all entities
    /// </summary>
    public interface IEntity : IDisposable
    {
        public Guid Id { get; set; }
    }
}
