using MediatR;
using PortfolioService.Core.Models;
using PortfolioService.Core.Models.Portfolio;

namespace PortfolioService.Core.Queries.Portfolio.GetPortfolio;

public record GetPortfolioQuery(string Id) : IRequest<Result<PortfolioDto>>;