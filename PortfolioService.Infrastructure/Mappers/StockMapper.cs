using PortfolioService.Domain.Entities;
using PortfolioService.Infrastructure.Documents;

namespace PortfolioService.Infrastructure.Mappers;

/// <summary>
/// Stock mapper.
/// </summary>
public static class StockMapper
{
    /// <summary>
    /// Map stock document to domain model.
    /// </summary>
    /// <param name="document">Stock document.</param>
    /// <returns>Stock domain model.</returns>
    public static Stock ToDomain(this StockDocument document)
    {
        return new Stock
        {
            Ticker = document.Ticker,
            BaseCurrency = document.BaseCurrency,
            NumberOfShares = document.NumberOfShares,
        };
    }

    /// <summary>
    /// Map list of stock documents to list of domain models.
    /// </summary>
    /// <param name="documents">List of stock documents.</param>
    /// <returns>List of stock domain models.</returns>
    public static IEnumerable<Stock> ToDomains(this IEnumerable<StockDocument> documents)
    {
        foreach (var document in documents)
        {
            yield return document.ToDomain();
        }
    }

    /// <summary>
    /// Map stock domain model to document.
    /// </summary>
    /// <param name="domain">Stock domain model.</param>
    /// <returns>Stock document.</returns>
    public static StockDocument ToDocument(this Stock domain)
    {
        return new StockDocument
        {
            Ticker = domain.Ticker,
            BaseCurrency = domain.BaseCurrency,
            NumberOfShares = domain.NumberOfShares,
        };
    }

    /// <summary>
    /// Map list of stock domain models to list of documents.
    /// </summary>
    /// <param name="domains">List of domain models.</param>
    /// <returns>List of stock documents.</returns>
    public static IEnumerable<StockDocument> ToDocuments(this IEnumerable<Stock> domains)
    {
        foreach (var domain in domains)
        {
            yield return domain.ToDocument();
        }
    }
}
