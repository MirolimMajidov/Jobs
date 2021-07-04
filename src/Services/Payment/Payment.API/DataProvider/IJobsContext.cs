using Jobs.Service.Common;
using PaymentService.Models;

namespace PaymentService.DataProvider
{
    public interface IJobsContext
    {
        public IEntityRepository<Payment> PaymentRepository { get; }
    }
}
