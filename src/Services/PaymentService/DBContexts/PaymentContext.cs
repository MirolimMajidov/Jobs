using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaymentService.Configurations;
using PaymentService.Models;
using PaymentService.Repository;
using Service.SharedModel.Repository;

namespace PaymentService.DBContexts
{
    public class PaymentContext
    {
        private readonly IMongoDatabase _database;
        private readonly DatabaseConfiguration _settings;

        public PaymentContext(IOptions<DatabaseConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
        }

        private IEntityRepository<PaymentHistory> _paymentHistoryRepository;

        public IEntityRepository<PaymentHistory> PaymentHistoryRepository
        {
            get
            {
                if (_paymentHistoryRepository == null)
                {
                    var payments = _database.GetCollection<PaymentHistory>(_settings.PaymentsCollectionName);
                    _paymentHistoryRepository = new EntityRepository<PaymentHistory>(payments);
                }

                return _paymentHistoryRepository;
            }
        }
    }
}
