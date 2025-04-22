using PortfolioService.Domain.Entities;

namespace PortfolioService.Domain.Repositories;

/// <summary>
/// Portfolio repository;
/// </summary>
public interface IPortfolioRepository
{
    /// <summary>
    /// Get portfolio by id.
    /// </summary>
    /// <param name="id">Portfolio id.</param>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>Portfolio.</returns>
    Task<Portfolio> GetByIdAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Soft delete portfolio by id.
    /// </summary>
    /// <param name="id">Portfolio id.</param>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    Task DeleteByIdAsync(string id, CancellationToken cancellationToken);
}
