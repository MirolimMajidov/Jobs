using Jobs.Service.Common;
using PaymentService.Models;

namespace PaymentService.DataProvider
{
    public interface IJobsMongoDBContext
    {
        public IEntityRepository<Payment> PaymentRepository { get; }
    }
}
