using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jobs.SharedModel.Models
{
    public abstract class IEntity : Disposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

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

        ~IEntity()
        {
            Dispose();
        }
    }
}
