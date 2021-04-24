using Newtonsoft.Json;
using System.ComponentModel;

namespace Jobs.SharedModel.Models
{
    public abstract class BaseEntity : IEntity
    {
        #region Properties

        [JsonIgnore]
        public bool Status { get; set; } = true;

        [DisplayName("Deleted")]
        [JsonIgnore]
        public bool IsDeleted { get; set; }

        #endregion

        #region Dispose

        ~BaseEntity()
        {
            Dispose();
        }

        #endregion
    }
}
