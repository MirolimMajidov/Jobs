using Jobs.Service.Common;
using PaymentService.Models;

namespace PaymentService.DataProvider
{
    public interface IJobsMongoContext
    {
        public IEntityRepository<Payment> PaymentRepository { get; }

        //If we want to add new table for MongoDB, we need to just to create a new collection repository with table name like PaymentRepository
    }
}
