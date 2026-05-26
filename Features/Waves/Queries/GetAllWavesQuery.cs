using ApiRefactor.Common;
using ApiRefactor.DTOs;
using MediatR;

namespace ApiRefactor.Features.Waves.Queries;

public sealed record GetAllWavesQuery : IRequest<Result<IEnumerable<WaveResponse>>>;
