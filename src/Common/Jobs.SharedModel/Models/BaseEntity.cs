using System.Text.Json.Serialization;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Jobs.SharedModel.Models
{
    /// <summary>
    /// Root entity of all entities
    /// </summary>
    public abstract class BaseEntity : Disposable, IEntity
    {
        #region Properties

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonIgnore]
        public bool Status { get; set; } = true;

        [DisplayName("Deleted")]
        [JsonIgnore]
        public bool IsDeleted { get; set; }

        public object this[string propertyName]
        {
            get
            {
                var propertyInfo = GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                    throw new ArgumentNullException(propertyName);

                return propertyInfo.GetValue(this);
            }
            set
            {
                var propertyInfo = GetType().GetProperty(propertyName);
                propertyInfo.SetValue(this, Convert.ChangeType(value, propertyInfo.PropertyType), null);
            }
        }

        #endregion

        #region Dispose

        ~BaseEntity()
        {
            Dispose();
        }

        #endregion
    }
}
