using MongoDB.Bson;
using PortfolioService.Domain.Entities;
using PortfolioService.Infrastructure.Documents;

namespace PortfolioService.Infrastructure.Mappers;

/// <summary>
/// Portfolio mapper.
/// </summary>
public static class PortfolioMapper
{
    /// <summary>
    /// Map portfolio document to domain model.
    /// </summary>
    /// <param name="document">Portfolio document.</param>
    /// <returns>Portfolio domain model.</returns>
    public static Portfolio ToDomain(this PortfolioDocument document)
    {
        return new Portfolio
        {
            Id = document.Id.ToString(),
            CurrentTotalValue = document.CurrentTotalValue,
            Stocks = document.Stocks != null ? document.Stocks.ToDomains().ToList() : new List<Stock>(),
        };
    }

    /// <summary>
    /// Map portfolio domain model to document.
    /// </summary>
    /// <param name="domain">Portfolio domain model.</param>
    /// <returns>Portfolio document.</returns>
    public static PortfolioDocument ToDocument(this Portfolio domain)
    {
        return new PortfolioDocument
        {
            Id = ObjectId.Parse(domain.Id),
            CurrentTotalValue = domain.CurrentTotalValue,
            Stocks = domain.Stocks != null ? domain.Stocks.ToDocuments().ToList() : new List<StockDocument>(),
        };
    }
}
