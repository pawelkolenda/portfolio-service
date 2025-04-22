using MediatR;
using Microsoft.Extensions.Logging;
using PortfolioService.Core.Models;
using PortfolioService.Core.Models.Portfolio;
using PortfolioService.Core.Services.CurrencyLayerApi;
using PortfolioService.Domain.Repositories;
using StockService;

namespace PortfolioService.Core.Queries.Portfolio.GetPortfolioStockValue;

public class GetPortfolioStockValueQueryHandler : IRequestHandler<GetPortfolioStockValueQuery, Result<PortfolioStockValueDto>>
{
    private readonly ILogger<GetPortfolioStockValueQueryHandler> _logger;
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly ICurrencyLayerApiClient _currencyLayerApiClient;
    private readonly IStockService _stockService;

    private Dictionary<string, decimal>? _usdRates;
    public const string CurrencyUsd = "USD";

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPortfolioStockValueQueryHandler"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="portfolioRepository">Portfolio repository.</param>
    /// <param name="currencyLayerApiClient">Currency layer API client.</param>
    /// <param name="stockService">Stock service.</param>
    public GetPortfolioStockValueQueryHandler(
        ILogger<GetPortfolioStockValueQueryHandler> logger,
        IPortfolioRepository portfolioRepository,
        ICurrencyLayerApiClient currencyLayerApiClient,
        IStockService stockService)
    {
        _logger = logger;
        _portfolioRepository = portfolioRepository;
        _currencyLayerApiClient = currencyLayerApiClient;
        _stockService = stockService;
    }

    /// <summary>
    /// Handles the get portfolio stock value query.
    /// </summary>
    /// <param name="request">The get portfolio stock value query.</param>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>A result containing the portfolio stock value or an error message.</returns>
    public async Task<Result<PortfolioStockValueDto>> Handle(GetPortfolioStockValueQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Id))
        {
            _logger.LogError("Could not get portfolio stock value for an empty id");
            return Result<PortfolioStockValueDto>.Fail($"Id could not be empty.");
        }

        if (string.IsNullOrEmpty(request.TargetCurrency))
        {
            _logger.LogError("Could not get portfolio stock value for an empty target currency");
            return Result<PortfolioStockValueDto>.Fail($"Target currency could not be empty.");
        }

        try
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(request.Id, cancellationToken);

            if (portfolio == null)
            {
                _logger.LogWarning("Could not get portfolio with id {Id}", request.Id);
                return Result<PortfolioStockValueDto>.Empty($"Could not get portfolio with id {request.Id}.");
            }

            if (portfolio.Stocks == null || portfolio.Stocks.Count == 0)
            {
                _logger.LogWarning("Portfolio with id {Id} does not contain any stocks", request.Id);
                return Result<PortfolioStockValueDto>.Empty($"Portfolio with id {request.Id} does not contain any stocks.");
            }

            var currencyLayerData = await _currencyLayerApiClient.GetLiveAsync(cancellationToken);
            if (currencyLayerData == null || !currencyLayerData.IsSuccess || currencyLayerData.ParsedResponse == null)
            {
                _logger.LogError("Failed to fetch currency layer data, error: {ErrorMessage}", currencyLayerData != null ? currencyLayerData.ErrorMessage : "empty response");
                return Result<PortfolioStockValueDto>.Fail($"Failed to fetch currency layer data.");
            }

            _usdRates = currencyLayerData.ParsedResponse.Quotes;
            if (_usdRates == null)
            {
                _logger.LogError("Fetched currency layer data has empty quotes");
                return Result<PortfolioStockValueDto>.Fail($"Fetched currency layer data has empty quotes.");
            }

            var stockPrices = await Task.WhenAll(portfolio.Stocks.Select(s => _stockService.GetCurrentStockPrice(s.Ticker)));
            if (stockPrices.All(v => v.Price == 0))
            {
                _logger.LogError("All fetched stock prices are zero");
                return Result<PortfolioStockValueDto>.Fail($"All fetched stock prices are zero.");
            }

            var usdToTargetCurrencyRate = request.TargetCurrency == CurrencyUsd ? 1 : GetUsdRate(request.TargetCurrency);

            decimal totalValue = 0;
            decimal usdToBaseCurrencyRate = 0;
            decimal priceInUsd = 0;

            foreach (var (stock, index) in portfolio.Stocks.Select((s, i) => (s, i)))
            {
                if (stock.BaseCurrency == request.TargetCurrency)
                {
                    totalValue += stockPrices[index].Price * stock.NumberOfShares;
                }
                else
                {
                    usdToBaseCurrencyRate = stock.BaseCurrency == CurrencyUsd ? 1 : GetUsdRate(stock.BaseCurrency);
                    priceInUsd = stockPrices[index].Price / usdToBaseCurrencyRate;

                    totalValue += request.TargetCurrency == CurrencyUsd
                        ? priceInUsd * stock.NumberOfShares
                        : priceInUsd * stock.NumberOfShares * usdToTargetCurrencyRate;
                }
            }

            return Result<PortfolioStockValueDto>.Ok(new PortfolioStockValueDto()
            {
                Id = request.Id,
                Currency = request.TargetCurrency,
                Value = totalValue
            });
        }
        catch (Exception exception)
        {
            _logger.LogError("Issue occured while getting portfolio stock value with id {Id} and currency {targetCurrency}, exception: {ExceptionMessage}", request.Id, request.TargetCurrency, exception.Message);
            return Result<PortfolioStockValueDto>.Fail($"Issue occured while getting portfolio stock value with id {request.Id} and currency {request.TargetCurrency}.");
        }
    }

    private decimal GetUsdRate(string targetCurrency)
    {
        var key = $"{CurrencyUsd}{targetCurrency}";
        try
        {
            return _usdRates[$"{key}"];
        }
        catch (KeyNotFoundException exception)
        {
            _logger.LogError($"Could not find {key} rate.");
            throw;
        }
    }
}
