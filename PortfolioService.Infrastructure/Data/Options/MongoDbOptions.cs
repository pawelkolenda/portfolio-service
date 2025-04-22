namespace PortfolioService.Infrastructure.Data.Options;

/// <summary>
/// Represents the options for configuring the database context.
/// </summary>
public class MongoDbOptions
{
    /// <summary>
    /// Gets or sets the connection string database.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the database name.
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;
}