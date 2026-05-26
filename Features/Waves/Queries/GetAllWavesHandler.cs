using ApiRefactor.Common;
using ApiRefactor.DTOs;
using ApiRefactor.Repositories;
using MediatR;

namespace ApiRefactor.Features.Waves.Queries;

public sealed class GetAllWavesHandler
    : IRequestHandler<GetAllWavesQuery, Result<PagedResponse<WaveResponse>>>
{
    private const int MaxPageSize = 100;

    private readonly IWaveRepository _repository;

    public GetAllWavesHandler(IWaveRepository repository) =>
        _repository = repository;

    public async Task<Result<PagedResponse<WaveResponse>>> Handle(
        GetAllWavesQuery request,
        CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, MaxPageSize);

        var (waves, totalCount) = await _repository.GetPagedAsync(page, pageSize, cancellationToken);

        var items = waves.Select(w => new WaveResponse(w.Id, w.Name, w.WaveDate));
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return Result<PagedResponse<WaveResponse>>.Success(
            new PagedResponse<WaveResponse>(items, page, pageSize, totalCount, totalPages));
    }
}
