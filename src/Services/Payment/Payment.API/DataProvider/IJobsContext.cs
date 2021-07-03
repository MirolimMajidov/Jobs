using Jobs.Service.Common.Repository;
using PaymentService.Models;

namespace PaymentService.DataProvider
{
    public interface IJobsContext
    {
        public IEntityRepository<Payment> PaymentRepository { get; }
    }
}
