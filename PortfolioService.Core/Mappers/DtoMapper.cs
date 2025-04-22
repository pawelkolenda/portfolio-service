using PortfolioService.Core.Models.Portfolio;
using PortfolioService.Domain.Entities;

namespace PortfolioService.Core.Mappers;

/// <summary>
/// Data transfer object mapper.
/// </summary>
public static class DtoMapper
{
    /// <summary>
    /// Map portfolio domain model to data transfer object.
    /// </summary>
    /// <param name="domain">Portfolio domain model.</param>
    /// <returns>Portfolio document.</returns>
    public static PortfolioDto ToDto(this Portfolio domain)
    {
        return new PortfolioDto
        {
            Id = domain.Id,
            CurrentTotalValue = domain.CurrentTotalValue,
            Stocks = domain.Stocks != null ? domain.Stocks.ToDtos().ToList() : new List<StockDto>(),
        };
    }

    /// <summary>
    /// Map stock domain model to data transfer object.
    /// </summary>
    /// <param name="domain">Stock domain model.</param>
    /// <returns>Stock data transfer object.</returns>
    public static StockDto ToDto(this Stock domain)
    {
        return new StockDto
        {
            Ticker = domain.Ticker,
            BaseCurrency = domain.BaseCurrency,
            NumberOfShares = domain.NumberOfShares,
        };
    }

    /// <summary>
    /// Map list of stock domain models to list of data transfer objects.
    /// </summary>
    /// <param name="domains">List of domain models.</param>
    /// <returns>List of stock data transfer objects.</returns>
    public static IEnumerable<StockDto> ToDtos(this IEnumerable<Stock> domains)
    {
        foreach (var domain in domains)
        {
            yield return domain.ToDto();
        }
    }
}
