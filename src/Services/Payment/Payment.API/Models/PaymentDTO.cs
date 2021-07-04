using Jobs.Service.Common;
using System;

namespace PaymentService.Models
{
    public class PaymentDTO : BaseEntityDTO
    {
        public Guid? UserId { get; set; }

        public string UserName { get; set; }

        public double Amount { get; set; }

        public CurrencyType Currency { get; set; } = CurrencyType.USD;

        public DateTime Date { get; set; } = DateTime.Now;

        public string Sender { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Process;
    }
}
