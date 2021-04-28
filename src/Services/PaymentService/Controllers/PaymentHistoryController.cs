using Microsoft.AspNetCore.Mvc;
using PaymentService.DBContexts;
using PaymentService.Models;
using Service.SharedModel.Controllers;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    public class PaymentHistoryController : BaseController<PaymentHistory>
    {
        public PaymentHistoryController(PaymentContext context) : base(context.PaymentHistoryRepository) { }
    }
}