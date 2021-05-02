using Microsoft.AspNetCore.Mvc;
using PaymentService.DataProvider;
using PaymentService.Models;
using Service.SharedModel.Controllers;
using Service.SharedModel.Helpers;
using System.Threading.Tasks;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : BaseController<Payment>
    {
        public PaymentController(JobsContext context) : base(context.PaymentRepository) { }

        public override async Task<RequestModel> Post([FromBody] Payment entity)
        {
            entity.UserId = User.GetUserId();

            return await base.Post(entity);
        }
    }
}