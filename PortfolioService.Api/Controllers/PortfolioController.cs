using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioService.Core.Commands.Portfolio.DeletePortfolio;
using PortfolioService.Core.Models.Portfolio;
using PortfolioService.Core.Queries.Portfolio.GetPortfolio;
using PortfolioService.Core.Queries.Portfolio.GetPortfolioStockValue;
using System.Net.Mime;

namespace PortfolioService.Api.Controllers;

/// <summary>
/// Controller for handling portfolio-related operations such as portfolio retrieval, stock value retrieval, portfolio delete.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class PortfolioController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="PortfolioController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance for sending commands and queries.</param>
    public PortfolioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves portfolio by id.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>An <see cref="IActionResult"/> containing the portfolio or an error message.</returns>
    [HttpGet("{id}", Name = nameof(GetPortfolio))]
    [ProducesResponseType(typeof(PortfolioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPortfolio([FromRoute] string id, CancellationToken cancellationToken)
    {
        var request = new GetPortfolioQuery(id);
        var result = await _mediator.Send(request, cancellationToken);
        if (result.Success)
        {
            if (result.Data == null)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    /// <summary>
    /// Retrieves portfolio stock value by id.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>An <see cref="IActionResult"/> containing the portfolio stock value or an error message.</returns>
    [HttpGet("{id}/stock-value", Name = nameof(GetPortfolioStockValue))]
    [ProducesResponseType(typeof(PortfolioStockValueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPortfolioStockValue([FromRoute] string id, [FromQuery] string currency, CancellationToken cancellationToken)
    {
        var request = new GetPortfolioStockValueQuery(id, currency);
        var result = await _mediator.Send(request, cancellationToken);
        if (result.Success)
        {
            if (result.Data == null)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    /// <summary>
    /// Delete portfolio by id.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token for the async operation.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpDelete("{id}", Name = nameof(DeletePortfolio))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePortfolio([FromRoute] string id, CancellationToken cancellationToken)
    {
        var request = new DeletePortfolioCommand(id);
        var result = await _mediator.Send(request, cancellationToken);
        if (result.Success)
        {
            return Ok();
        }
        return BadRequest(result.Message);
    }
}