using PortfolioService.Core.Models.CurrencyLayerApi;

namespace PortfolioService.Core.Services.CurrencyLayerApi;

/// <summary>
/// Client for interacting with the currency layer API to fetch live.
/// </summary>
public interface ICurrencyLayerApiClient
{
    /// <summary>
    /// Fetches the live from the currency layer API.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>The result of the API call containing live currency details.</returns>
    Task<CurrencyLayerApiLiveResult> GetLiveAsync(CancellationToken cancellationToken);
}
