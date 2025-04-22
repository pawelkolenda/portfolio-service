namespace PortfolioService.Core.Models.Portfolio;

/// <summary>
/// Stock data transfer object.
/// </summary>
public class PortfolioStockValueDto
{
    /// <summary>
    /// Gets or sets portfolio id.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets currency.
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets value of stock.
    /// </summary>
    public decimal Value { get; set; }
}
