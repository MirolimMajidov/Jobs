using Jobs.SharedModel.Helpers;
using Jobs.SharedModel.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.Models
{
    public class PaymentHistory : Disposable, IEntity
    {
        [BsonId]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public string OrderId { get; set; }

        public double Amount { get; set; } = 0;

        public CurrencyType Currency { get; set; } = CurrencyType.USD;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime Date { get; set; } = DateTime.Now;

        public string Sender { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Process;
    }
}
