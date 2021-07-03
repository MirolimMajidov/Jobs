using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaymentService.Configurations;
using PaymentService.Models;
using PaymentService.Repository;
using Jobs.Service.Common.Repository;

namespace PaymentService.DataProvider
{
    public class JobsContext
    {
        private readonly IMongoDatabase _database;
        private readonly DatabaseConfiguration _settings;

        public JobsContext(IOptions<DatabaseConfiguration> settings)
        {
            if (settings != null)
            {
                _settings = settings.Value;
                var client = new MongoClient(_settings.ConnectionString);
                _database = client.GetDatabase(_settings.DatabaseName);
            }
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
#if DEBUG
            set
            {
                _paymentRepository = value;
            }
#endif
        }
    }
}
