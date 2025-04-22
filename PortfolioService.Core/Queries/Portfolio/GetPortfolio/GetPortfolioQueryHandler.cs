using MediatR;
using Microsoft.Extensions.Logging;
using PortfolioService.Core.Mappers;
using PortfolioService.Core.Models;
using PortfolioService.Core.Models.Portfolio;
using PortfolioService.Domain.Repositories;

namespace PortfolioService.Core.Queries.Portfolio.GetPortfolio;

/// <summary>
/// Handles the query to retrive portfolio by id.
/// </summary>
public class GetPortfolioQueryHandler : IRequestHandler<GetPortfolioQuery, Result<PortfolioDto>>
{
    private readonly ILogger<GetPortfolioQueryHandler> _logger;
    private readonly IPortfolioRepository _portfolioRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPortfolioQueryHandler"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="portfolioRepository">Portfolio repository.</param>
    public GetPortfolioQueryHandler(ILogger<GetPortfolioQueryHandler> logger, IPortfolioRepository portfolioRepository)
    {
        _logger = logger;
        _portfolioRepository = portfolioRepository;
    }

    /// <summary>
    /// Handles the get portfolio query.
    /// </summary>
    /// <param name="request">The get portfolio query.</param>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>A result containing the portfolio or an error message.</returns>
    public async Task<Result<PortfolioDto>> Handle(GetPortfolioQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Id))
        {
            _logger.LogError("Could not get portfolio for an empty id");
            return Result<PortfolioDto>.Fail($"Id could not be empty.");
        }

        try
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(request.Id, cancellationToken);

            if (portfolio == null)
            {
                _logger.LogWarning("Could not get portfolio with id {Id}", request.Id);
                return Result<PortfolioDto>.Empty($"Could not get portfolio with id {request.Id}.");
            }

            return Result<PortfolioDto>.Ok(portfolio.ToDto());
        }
        catch (Exception exception)
        {
            _logger.LogError("Issue occured while getting portfolio with id {Id}, exception: {ExceptionMessage}", request.Id, exception.Message);
            return Result<PortfolioDto>.Fail($"Issue occured while getting portfolio with id {request.Id}.");
        }
    }
}