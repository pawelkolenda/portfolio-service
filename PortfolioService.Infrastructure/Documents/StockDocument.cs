using MongoDB.Bson.Serialization.Attributes;

namespace PortfolioService.Infrastructure.Documents;

/// <summary>
/// Stock MongoDB document representation.
/// </summary>
public class StockDocument
{
    /// <summary>
    /// Gets or sets ticker.
    /// </summary>
    [BsonElement("ticker")]
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets base currency.
    /// </summary>
    [BsonElement("baseCurrency")]
    public string BaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets number of shares.
    /// </summary>
    [BsonElement("numberOfShares")]
    public int NumberOfShares { get; set; }
}
