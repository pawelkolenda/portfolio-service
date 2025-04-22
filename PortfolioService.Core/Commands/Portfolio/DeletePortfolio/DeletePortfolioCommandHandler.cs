using MediatR;
using Microsoft.Extensions.Logging;
using PortfolioService.Core.Models;
using PortfolioService.Core.Models.Portfolio;
using PortfolioService.Domain.Repositories;

namespace PortfolioService.Core.Commands.Portfolio.DeletePortfolio;

public class DeletePortfolioCommandHandler : IRequestHandler<DeletePortfolioCommand, Result>
{
    private readonly ILogger<DeletePortfolioCommandHandler> _logger;
    private readonly IPortfolioRepository _portfolioRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePortfolioCommandHandler"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="portfolioRepository">Portfolio repository.</param>
    public DeletePortfolioCommandHandler(ILogger<DeletePortfolioCommandHandler> logger, IPortfolioRepository portfolioRepository)
    {
        _logger = logger;
        _portfolioRepository = portfolioRepository;
    }

    /// <summary>
    /// Handles the delete portfolio command.
    /// </summary>
    /// <param name="request">The delete portfolio command.</param>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> Handle(DeletePortfolioCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Id))
        {
            _logger.LogError("Could not delete portfolio for an empty id");
            return Result<PortfolioDto>.Fail($"Id could not be empty.");
        }

        try
        {
            await _portfolioRepository.DeleteByIdAsync(request.Id, cancellationToken);
            return Result.Ok();
        }
        catch (Exception exception)
        {
            _logger.LogError("Issue occured while deleting portfolio with id: {Id}, exception: {ExceptionMessage}", request.Id, exception.Message);
            return Result<PortfolioDto>.Fail($"Issue occured while deleting portfolio with id: {request.Id}.");
        }
    }
}
