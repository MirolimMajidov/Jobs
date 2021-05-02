using Microsoft.AspNetCore.Mvc;
using PaymentService.DataProvider;
using PaymentService.Models;
using Service.SharedModel.Controllers;
using Service.SharedModel.Helpers;
using System.Threading.Tasks;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    public class PaymentHistoryController : BaseController<PaymentHistory>
    {
        public PaymentHistoryController(JobsContext context) : base(context.PaymentHistoryRepository) { }

        public override async Task<RequestModel> Post([FromBody] PaymentHistory entity)
        {
            entity.UserId = User.GetUserId();

            return await base.Post(entity);
        }
    }
}