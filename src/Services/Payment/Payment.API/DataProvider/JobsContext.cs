using Jobs.Service.Common;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaymentService.Configurations;
using PaymentService.Models;
using PaymentService.Repository;

namespace PaymentService.DataProvider
{
    public class JobsContext : IJobsContext
    {
        private readonly IMongoDatabase _database;
        private readonly DatabaseConfiguration _settings;

        public JobsContext(IOptions<DatabaseConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
        }

        private IEntityRepository<Payment> _paymentRepository;

        public IEntityRepository<Payment> PaymentRepository
        {
            get
            {
                if (_paymentRepository == null)
                {
                    var payments = _database.GetCollection<Payment>(_settings.PaymentsName);
                    _paymentRepository = new EntityRepository<Payment>(payments);
                }

                return _paymentRepository;
            }
        }
    }
}
