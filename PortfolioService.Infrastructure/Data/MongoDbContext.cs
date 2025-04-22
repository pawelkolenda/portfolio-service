using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PortfolioService.Infrastructure.Data.Options;
using PortfolioService.Infrastructure.Documents;

namespace PortfolioService.Infrastructure.Data;
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<PortfolioDocument> Portfolios => _database.GetCollection<PortfolioDocument>("Portfolios");
}