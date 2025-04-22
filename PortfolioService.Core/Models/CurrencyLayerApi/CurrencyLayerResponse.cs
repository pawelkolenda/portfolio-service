namespace PortfolioService.Core.Models.CurrencyLayerApi
{
    /// <summary>
    /// Represents a response returned by the currency layer API.
    /// </summary>
    public class CurrencyLayerResponse
    {
        /// <summary>
        /// Gets or sets success flag.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets terms.
        /// </summary>
        public string? Terms { get; set; }

        /// <summary>
        /// Gets or sets privacy.
        /// </summary>
        public string? Privacy { get; set; }

        /// <summary>
        /// Gets or sets timestamp.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Gets or sets source.
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// Gets or sets quotes.
        /// </summary>
        public Dictionary<string, decimal>? Quotes { get; set; }
    }
}
