using Jobs.Service.Common;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaymentService.Configurations;
using PaymentService.Models;
using PaymentService.Repository;

namespace PaymentService.DataProvider
{
    public class JobsMongoContext : IJobsMongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly DatabaseConfiguration _settings;

        public JobsMongoContext(IOptions<DatabaseConfiguration> settings)
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
                    var collectionName = $"{nameof(Payment)}s";
                    var payments = _database.GetCollection<Payment>(collectionName);
                    _paymentRepository = new MongoEntityRepository<Payment>(payments);
                }

                return _paymentRepository;
            }
        }

        //If we want to add new table for MongoDB, we need to just to create a new collection repository with table name like PaymentRepository
    }
}
