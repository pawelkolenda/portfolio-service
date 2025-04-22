namespace PortfolioService.Core.Models.Portfolio;

/// <summary>
/// Portfolio data transfer object.
/// </summary>
public class PortfolioDto
{
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets current total value.
    /// </summary>
    public decimal CurrentTotalValue { get; set; }

    /// <summary>
    /// Gets or sets list of stocks.
    /// </summary>
    public ICollection<StockDto>? Stocks { get; set; }
}
