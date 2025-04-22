using MongoDB.Bson;
using MongoDB.Driver;
using PortfolioService.Domain.Entities;
using PortfolioService.Domain.Repositories;
using PortfolioService.Infrastructure.Data;
using PortfolioService.Infrastructure.Documents;
using PortfolioService.Infrastructure.Mappers;

namespace PortfolioService.Infrastructure.Repositories;

/// <inheritdoc/>
public class PortfolioRepository : IPortfolioRepository
{
    private readonly IMongoCollection<PortfolioDocument> _portfolioCollection;

    /// <summary>
    /// Initializes a new instance of the <see cref="PortfolioRepository"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    public PortfolioRepository(MongoDbContext context)
    {
        _portfolioCollection = context.Portfolios;
    }

    /// <inheritdoc/>
    public async Task<Portfolio> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<PortfolioDocument>.Filter.And(
            Builders<PortfolioDocument>.Filter.Eq(p => p.Id, ObjectId.Parse(id)),
            Builders<PortfolioDocument>.Filter.Or(
                Builders<PortfolioDocument>.Filter.Eq(p => p.IsDeleted, false),
                Builders<PortfolioDocument>.Filter.Eq(p => p.IsDeleted, null)
            )
        );
        var portfolioData = await _portfolioCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return portfolioData?.ToDomain();
    }

    /// <inheritdoc/>
    public async Task DeleteByIdAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<PortfolioDocument>.Filter.Eq(p => p.Id, ObjectId.Parse(id));
        var update = Builders<PortfolioDocument>.Update.Set(p => p.IsDeleted, true);
        await _portfolioCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }
}
