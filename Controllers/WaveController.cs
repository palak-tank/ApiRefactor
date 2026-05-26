using ApiRefactor.DTOs;
using ApiRefactor.Features.Waves.Commands;
using ApiRefactor.Features.Waves.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiRefactor.Controllers;

[ApiController]
[Route("api/wave")]
[Authorize]
public sealed class WaveController : ControllerBase
{
    private readonly IMediator _mediator;

    public WaveController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<WaveResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllWavesQuery(page, pageSize), cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WaveResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetWaveByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = "writer")]
    [ProducesResponseType(typeof(WaveResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateWaveRequest request)
    {
        var result = await _mediator.Send(new CreateWaveCommand(request.Name, request.WaveDate));

        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = result.Error });

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value!.Id },
            result.Value);
    }
}
