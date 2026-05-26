using ApiRefactor.Common;
using ApiRefactor.DTOs;
using MediatR;

namespace ApiRefactor.Features.Waves.Queries;

public sealed record GetWaveByIdQuery(Guid Id) : IRequest<Result<WaveResponse?>>;
