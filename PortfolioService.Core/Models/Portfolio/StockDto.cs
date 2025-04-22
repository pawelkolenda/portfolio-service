namespace PortfolioService.Core.Models.Portfolio;

/// <summary>
/// Stock data transfer object.
/// </summary>
public class StockDto
{
    /// <summary>
    /// Gets or sets ticker.
    /// </summary>
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets base currency.
    /// </summary>
    public string BaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets number of shares.
    /// </summary>
    public int NumberOfShares { get; set; }
}
