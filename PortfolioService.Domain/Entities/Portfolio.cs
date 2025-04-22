namespace PortfolioService.Domain.Entities;

/// <summary>
/// Portfolio.
/// </summary>
public class Portfolio
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
    public ICollection<Stock>? Stocks { get; set; }

    /// <summary>
    /// Gets or sets is delete flag.
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}
