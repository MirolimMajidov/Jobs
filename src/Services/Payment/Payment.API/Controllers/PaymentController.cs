using Microsoft.AspNetCore.Mvc;
using PaymentService.DataProvider;
using PaymentService.Models;
using Jobs.Service.Common.Controllers;
using Jobs.Service.Common.Helpers;
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
            entity.UserName = User.GetUserName();

            return await base.Post(entity);
        }
    }
}