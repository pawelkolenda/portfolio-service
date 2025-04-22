namespace PortfolioService.Api.Options.CurrencyLayerApi;

/// <summary>
/// Represents the configuration options for accessing the currency layer API.
/// </summary>
public class CurrencyLayerApiOptions
{
    /// <summary>
    /// Gets or sets url of the API.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets api access key.
    /// </summary>
    public string ApiAccessKey { get; set; } = string.Empty;
}
