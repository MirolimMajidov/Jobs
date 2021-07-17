using Jobs.Service.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.Models
{
    public class TransactionDTO : BaseEntityDTO
    {
        [Required]
        public Guid? UserId { get; set; }

        public string UserName { get; set; }

        public double Amount { get; set; } = 0;

        public CurrencyType Currency { get; set; } = CurrencyType.USD;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime Date { get; set; } = DateTime.Now;

        public string Receiver { get; set; }
    }
}
