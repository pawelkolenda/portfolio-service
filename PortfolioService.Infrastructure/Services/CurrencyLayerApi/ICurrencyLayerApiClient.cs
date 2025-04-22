using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PortfolioService.Api.Options.CurrencyLayerApi;
using PortfolioService.Core.Models.CurrencyLayerApi;
using PortfolioService.Core.Services.CurrencyLayerApi;

namespace PortfolioService.Infrastructure.Services.CurrencyLayerApi;

/// <inheritdoc/>
public class CurrencyLayerApiClient : ICurrencyLayerApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CurrencyLayerApiClient> _logger;
    private readonly CurrencyLayerApiOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrencyLayerApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="options">The external booking API options.</param>
    public CurrencyLayerApiClient(HttpClient httpClient, ILogger<CurrencyLayerApiClient> logger, IOptions<CurrencyLayerApiOptions> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc/>
    public async Task<CurrencyLayerApiLiveResult> GetLiveAsync(CancellationToken cancellationToken)
    {
        var url = $"{_options.Url}live?access_key={_options.ApiAccessKey}";

        try
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Currency Layer API returned non-success status for getting data: {StatusCode}.", response.StatusCode);
                return CurrencyLayerApiLiveResult.Failure(content, $"HTTP {response.StatusCode}, content: {content}");
            }

            _logger.LogInformation("Successfully fetched data from currency layer.");
            return CurrencyLayerApiLiveResult.Success(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch data from currency layer.");
            return CurrencyLayerApiLiveResult.Failure(null, ex.Message);
        }
    }
}
