using MediatR;
using PortfolioService.Core.Models;

namespace PortfolioService.Core.Commands.Portfolio.DeletePortfolio;

public record DeletePortfolioCommand(string Id) : IRequest<Result>;