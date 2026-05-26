using ApiRefactor.Common;
using ApiRefactor.DTOs;
using MediatR;

namespace ApiRefactor.Features.Waves.Commands;

public sealed record CreateWaveCommand(string Name, DateTime WaveDate)
    : IRequest<Result<WaveResponse>>;
