using MediatR;
using PortfolioService.Core.Models;
using PortfolioService.Core.Models.Portfolio;

namespace PortfolioService.Core.Queries.Portfolio.GetPortfolioStockValue;

public record GetPortfolioStockValueQuery(string Id, string TargetCurrency) : IRequest<Result<PortfolioStockValueDto>>;