using ApiRefactor.DTOs;
using ApiRefactor.Features.Waves.Commands;
using ApiRefactor.Features.Waves.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiRefactor.Controllers;

[ApiController]
[Route("api/wave")]
public sealed class WaveController : ControllerBase
{
    private readonly IMediator _mediator;

    public WaveController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<WaveResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllWavesQuery());
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
    [ProducesResponseType(typeof(WaveResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateWaveRequest request)
    {
        var result = await _mediator.Send(new CreateWaveCommand(request.Name, request.WaveDate));

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value!.Id },
            result.Value);
    }
}
