using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using PortfolioService.Core.Models.CurrencyLayerApi;
using PortfolioService.Core.Queries.Portfolio.GetPortfolioStockValue;
using PortfolioService.Core.Services.CurrencyLayerApi;
using PortfolioService.Domain.Repositories;
using Shouldly;
using StockService;
using Xunit;

namespace PortfolioService.Core.UnitTests.Portfolio;

public class GetPortfolioStockValueQueryHandlerTests
{
    private readonly Mock<ILogger<GetPortfolioStockValueQueryHandler>> _loggerMock;
    private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock;
    private readonly Mock<ICurrencyLayerApiClient> _currencyLayerApiClientMock;
    private readonly Mock<IStockService> _stockServiceMock;

    public GetPortfolioStockValueQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetPortfolioStockValueQueryHandler>>();
        _portfolioRepositoryMock = new Mock<IPortfolioRepository>();
        _currencyLayerApiClientMock = new Mock<ICurrencyLayerApiClient>();
        _stockServiceMock = new Mock<IStockService>();
    }

    private GetPortfolioStockValueQueryHandler CreateHandler()
    {
        return new GetPortfolioStockValueQueryHandler(
            _loggerMock.Object, 
            _portfolioRepositoryMock.Object, 
            _currencyLayerApiClientMock.Object, 
            _stockServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRequestIdIsEmpty_ReturnsFailWithErrorMessage()
    {
        // Arrange
        var query = new GetPortfolioStockValueQuery(string.Empty, string.Empty);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe("Id could not be empty.");
        result.Data.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenRequestTargetCurrencyIsEmpty_ReturnsFailWithErrorMessage()
    {
        // Arrange
        var id = "1";

        var query = new GetPortfolioStockValueQuery(id, string.Empty);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe("Target currency could not be empty.");
        result.Data.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenPortfolioNotExists_ReturnsEmptyWithErrorMessage()
    {
        // Arrange
        var id = "1";
        var currency = "USD";

        var query = new GetPortfolioStockValueQuery(id, currency);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeTrue();
        result.Message.ShouldBe($"Could not get portfolio with id {id}.");
        result.Data.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenPortfolioExistsWithoutStocks_ReturnsEmptyWithErrorMessage()
    {
        // Arrange
        var id = "1";
        var currency = "USD";

        var entity = new Domain.Entities.Portfolio()
        {
            Id = id,
            CurrentTotalValue = 0,
            IsDeleted = false,
            Stocks = new List<Domain.Entities.Stock>()
        };

        _portfolioRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var query = new GetPortfolioStockValueQuery(id, currency);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeTrue();
        result.Message.ShouldBe($"Portfolio with id {id} does not contain any stocks.");
        result.Data.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenCurrencyLayerDataIsNull_ReturnsFailWithErrorMessage()
    {
        // Arrange
        var id = "1";
        var currency = "USD";

        var entity = new Domain.Entities.Portfolio() {
            Id = id,
            CurrentTotalValue = 0,
            IsDeleted = false,
            Stocks = new List<Domain.Entities.Stock>()
            {
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "PLN",
                    NumberOfShares = 1,
                    Ticker = "T1",
                },
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "USD",
                    NumberOfShares = 1,
                    Ticker = "T2",
                },
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "CAD",
                    NumberOfShares = 2,
                    Ticker = "T3",
                },
            },
        };

        _portfolioRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var query = new GetPortfolioStockValueQuery(id, currency);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe($"Failed to fetch currency layer data.");
        result.Data.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenCurrencyLayerDataIsNotSuccess_ReturnsFailWithErrorMessage()
    {
        // Arrange
        var id = "1";
        var currency = "USD";

        var entity = new Domain.Entities.Portfolio()
        {
            Id = id,
            CurrentTotalValue = 0,
            IsDeleted = false,
            Stocks = new List<Domain.Entities.Stock>()
            {
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "PLN",
                    NumberOfShares = 1,
                    Ticker = "T1",
                },
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "USD",
                    NumberOfShares = 1,
                    Ticker = "T2",
                },
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "CAD",
                    NumberOfShares = 2,
                    Ticker = "T3",
                },
            },
        };

        _portfolioRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var currencyLayerApiFailureResponse = CurrencyLayerApiLiveResult.Failure(string.Empty, "Error message");

        _currencyLayerApiClientMock
            .Setup(x => x.GetLiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencyLayerApiFailureResponse);

        var query = new GetPortfolioStockValueQuery(id, currency);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe($"Failed to fetch currency layer data.");
        result.Data.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenCurrencyLayerDataHasNoQuotes_ReturnsFailWithErrorMessage()
    {
        // Arrange
        var id = "1";
        var currency = "USD";

        var entity = new Domain.Entities.Portfolio()
        {
            Id = id,
            CurrentTotalValue = 0,
            IsDeleted = false,
            Stocks = new List<Domain.Entities.Stock>()
            {
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "PLN",
                    NumberOfShares = 1,
                    Ticker = "T1",
                },
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "USD",
                    NumberOfShares = 1,
                    Ticker = "T2",
                },
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "CAD",
                    NumberOfShares = 2,
                    Ticker = "T3",
                },
            },
        };

        _portfolioRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var currencyLayerApiSuccessEmptyResponse = CurrencyLayerApiLiveResult.Success(GetCurrencyLayerApiGetLiveAsyncEmptyResponse());

        _currencyLayerApiClientMock
            .Setup(x => x.GetLiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencyLayerApiSuccessEmptyResponse);

        var query = new GetPortfolioStockValueQuery(id, currency);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe($"Fetched currency layer data has empty quotes.");
        result.Data.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenAllStockPricesAreZero_ReturnsFailWithErrorMessage()
    {
        // Arrange
        var id = "1";
        var currency = "PLN";

        var entity = new Domain.Entities.Portfolio()
        {
            Id = id,
            CurrentTotalValue = 0,
            IsDeleted = false,
            Stocks = new List<Domain.Entities.Stock>()
            {
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "PLN",
                    NumberOfShares = 1,
                    Ticker = "T1",
                },
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "USD",
                    NumberOfShares = 1,
                    Ticker = "T2",
                },
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "CAD",
                    NumberOfShares = 2,
                    Ticker = "T3",
                },
            },
        };

        _portfolioRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var currencyLayerApiSuccessResponse = CurrencyLayerApiLiveResult.Success(GetCurrencyLayerApiGetLiveAsyncResponse());

        _currencyLayerApiClientMock
            .Setup(x => x.GetLiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencyLayerApiSuccessResponse);

        var query = new GetPortfolioStockValueQuery(id, currency);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe($"All fetched stock prices are zero.");
        result.Data.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenRequestHasIncorrectCurrency_ReturnsFailWithErrorMessage()
    {
        // Arrange
        var id = "1";
        var currency = "INCORRECT CURRENCY";

        var entity = new Domain.Entities.Portfolio()
        {
            Id = id,
            CurrentTotalValue = 0,
            IsDeleted = false,
            Stocks = new List<Domain.Entities.Stock>()
            {
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "PLN",
                    NumberOfShares = 1,
                    Ticker = "T1",
                },
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "USD",
                    NumberOfShares = 1,
                    Ticker = "T2",
                },
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "CAD",
                    NumberOfShares = 2,
                    Ticker = "T3",
                },
            },
        };

        _portfolioRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var currencyLayerApiSuccessResponse = CurrencyLayerApiLiveResult.Success(GetCurrencyLayerApiGetLiveAsyncResponse());

        _currencyLayerApiClientMock
            .Setup(x => x.GetLiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencyLayerApiSuccessResponse);

        _stockServiceMock
            .Setup(x => x.GetCurrentStockPrice("T1"))
            .ReturnsAsync((10, "PLN"));

        _stockServiceMock
            .Setup(x => x.GetCurrentStockPrice("T2"))
            .ReturnsAsync((20, "USD"));

        _stockServiceMock
            .Setup(x => x.GetCurrentStockPrice("T3"))
            .ReturnsAsync((30, "CAD"));

        var query = new GetPortfolioStockValueQuery(id, currency);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.Message.ShouldBe($"Issue occured while getting portfolio stock value with id {id} and currency {currency}.");
        result.Data.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenCorrectRequestSent_ReturnsSuccessWithCorrectCalculations()
    {
        // Arrange
        var id = "1";
        var currency = "PLN";

        // CURRENCY -> CURRENCY = SHARES * STOCK PRICE * (CURRENCY RATE)
        var entity = new Domain.Entities.Portfolio()
        {
            Id = id,
            CurrentTotalValue = 0,
            IsDeleted = false,
            Stocks = new List<Domain.Entities.Stock>()
            {
                // PLN -> PLN = 1 * 10 * 1 = 10
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "PLN",
                    NumberOfShares = 1,
                    Ticker = "T1",
                },
                // USD -> PLN = 1 * 20 * (4 / 1) = 80
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "USD",
                    NumberOfShares = 1,
                    Ticker = "T2",
                },
                // CAD -> PLN = 2 * 30 * (4 / 2) = 120
                new Domain.Entities.Stock()
                {
                    BaseCurrency = "CAD",
                    NumberOfShares = 2,
                    Ticker = "T3",
                },
            },
        };

        _portfolioRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var currencyLayerApiSuccessResponse = CurrencyLayerApiLiveResult.Success(GetCurrencyLayerApiGetLiveAsyncResponse());

        _currencyLayerApiClientMock
            .Setup(x => x.GetLiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencyLayerApiSuccessResponse);

        _stockServiceMock
            .Setup(x => x.GetCurrentStockPrice("T1"))
            .ReturnsAsync((10, "PLN"));

        _stockServiceMock
            .Setup(x => x.GetCurrentStockPrice("T2"))
            .ReturnsAsync((20, "USD"));

        _stockServiceMock
            .Setup(x => x.GetCurrentStockPrice("T3"))
            .ReturnsAsync((30, "CAD"));

        var query = new GetPortfolioStockValueQuery(id, currency);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeTrue();
        result.Message.ShouldBeNull();
        result.Data.ShouldNotBeNull();
        result.Data.Id.ShouldBe(id);
        result.Data.Currency.ShouldBe(currency);
        result.Data.Value.ShouldBe(210);
    }

    private string GetCurrencyLayerApiGetLiveAsyncEmptyResponse()
    {
        return """
                        {
                          "success": true,
                          "terms": "https://currencylayer.com/terms",
                          "privacy": "https://currencylayer.com/privacy",
                          "timestamp": 1745337549,
                          "source": "USD"
                        }
            """;
    }

    private string GetCurrencyLayerApiGetLiveAsyncResponse()
    {
        return """
                        {
                          "success": true,
                          "terms": "https://currencylayer.com/terms",
                          "privacy": "https://currencylayer.com/privacy",
                          "timestamp": 1745337549,
                          "source": "USD",
                          "quotes": {
                            "USDPLN": 4,
                            "USDCAD": 2
                          }
                        }
            """;
    }
}


