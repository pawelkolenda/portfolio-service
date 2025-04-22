using System.Text.Json;

namespace PortfolioService.Core.Models.CurrencyLayerApi
{
    /// <summary>
    /// Represents the result of a request to the currency layer API for fetching live.
    /// </summary>
    public class CurrencyLayerApiLiveResult
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        private CurrencyLayerApiLiveResult(bool isSuccess, string? rawResponse, CurrencyLayerResponse? parsedResponse, string? errorMessage)
        {
            IsSuccess = isSuccess;
            RawResponse = rawResponse;
            ParsedResponse = parsedResponse;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets a value indicating whether the API request was successful.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Gets the raw JSON response from the API.
        /// </summary>
        public string? RawResponse { get; private set; }

        /// <summary>
        /// Gets the parsed response from the API.
        /// </summary>
        public CurrencyLayerResponse? ParsedResponse { get; private set; }

        /// <summary>
        /// Gets the error message if the API request failed.
        /// </summary>
        public string? ErrorMessage { get; private set; }

        /// <summary>
        /// Creates a successful result from the raw API response.
        /// </summary>
        /// <param name="rawResponse">The raw JSON response from the API.</param>
        /// <returns>A successful <see cref="CurrencyLayerApiLiveResult"/> instance.</returns>
        public static CurrencyLayerApiLiveResult Success(string rawResponse)
        {
            var parsedResponse = JsonSerializer.Deserialize<CurrencyLayerResponse>(rawResponse, JsonOptions);
            return new CurrencyLayerApiLiveResult(true, rawResponse, parsedResponse, null);
        }

        /// <summary>
        /// Creates a failed result with an error message.
        /// </summary>
        /// <param name="rawResponse">The raw JSON response from the API, if any.</param>
        /// <param name="error">The error message.</param>
        /// <returns>A failed <see cref="CurrencyLayerApiLiveResult"/> instance.</returns>
        public static CurrencyLayerApiLiveResult Failure(string? rawResponse, string error)
        {
            return new CurrencyLayerApiLiveResult(false, rawResponse, null, error);
        }
    }
}
