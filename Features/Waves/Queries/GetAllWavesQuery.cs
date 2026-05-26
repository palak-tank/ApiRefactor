using ApiRefactor.Common;
using ApiRefactor.DTOs;
using MediatR;

namespace ApiRefactor.Features.Waves.Queries;

public sealed record GetAllWavesQuery(int Page, int PageSize)
    : IRequest<Result<PagedResponse<WaveResponse>>>;
