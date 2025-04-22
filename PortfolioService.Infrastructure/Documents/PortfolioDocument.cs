using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PortfolioService.Infrastructure.Documents;

/// <summary>
/// Portfolio MongoDB document representation.
/// </summary>
public class PortfolioDocument
{
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    [BsonElement("_id")]
    public ObjectId Id { get; set; }

    /// <summary>
    /// Gets or sets current total value.
    /// </summary>
    [BsonElement("currentTotalValue")]
    public decimal CurrentTotalValue { get; set; }

    /// <summary>
    /// Gets or sets list of stocks.
    /// </summary>
    [BsonElement("stocks")]
    public ICollection<StockDocument>? Stocks { get; set; }

    /// <summary>
    /// Gets or sets is delete flag.
    /// </summary>
    [BsonElement("isDeleted")]
    public bool? IsDeleted { get; set; } = false;
}
