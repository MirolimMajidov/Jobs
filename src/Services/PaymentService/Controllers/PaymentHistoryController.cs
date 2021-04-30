using Microsoft.AspNetCore.Mvc;
using PaymentService.DataProvider;
using PaymentService.Models;
using Service.SharedModel.Controllers;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    public class PaymentHistoryController : BaseController<PaymentHistory>
    {
        public PaymentHistoryController(JobsContext context) : base(context.PaymentHistoryRepository) { }
    }
}