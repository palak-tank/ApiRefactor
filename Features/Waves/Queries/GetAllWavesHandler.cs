using ApiRefactor.Common;
using ApiRefactor.DTOs;
using ApiRefactor.Repositories;
using MediatR;

namespace ApiRefactor.Features.Waves.Queries;

public sealed class GetAllWavesHandler
    : IRequestHandler<GetAllWavesQuery, Result<IEnumerable<WaveResponse>>>
{
    private readonly IWaveRepository _repository;

    public GetAllWavesHandler(IWaveRepository repository) =>
        _repository = repository;

    public async Task<Result<IEnumerable<WaveResponse>>> Handle(
        GetAllWavesQuery request,
        CancellationToken cancellationToken)
    {
        var waves = await _repository.GetAllAsync();
        var response = waves.Select(w => new WaveResponse(w.Id, w.Name, w.WaveDate));
        return Result<IEnumerable<WaveResponse>>.Success(response);
    }
}
