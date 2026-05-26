using ApiRefactor.Common;
using ApiRefactor.DTOs;
using ApiRefactor.Repositories;
using MediatR;

namespace ApiRefactor.Features.Waves.Queries;

public sealed class GetWaveByIdHandler
    : IRequestHandler<GetWaveByIdQuery, Result<WaveResponse?>>
{
    private readonly IWaveRepository _repository;

    public GetWaveByIdHandler(IWaveRepository repository) =>
        _repository = repository;

    public async Task<Result<WaveResponse?>> Handle(
        GetWaveByIdQuery request,
        CancellationToken cancellationToken)
    {
        var wave = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (wave is null)
            return Result<WaveResponse?>.Failure($"Wave with ID {request.Id} was not found.");

        return Result<WaveResponse?>.Success(new WaveResponse(wave.Id, wave.Name, wave.WaveDate));
    }
}
