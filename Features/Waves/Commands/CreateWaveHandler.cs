using ApiRefactor.Common;
using ApiRefactor.DTOs;
using ApiRefactor.Models;
using ApiRefactor.Repositories;
using MediatR;

namespace ApiRefactor.Features.Waves.Commands;

public sealed class CreateWaveHandler : IRequestHandler<CreateWaveCommand, Result<WaveResponse>>
{
    private readonly IWaveRepository _repository;

    public CreateWaveHandler(IWaveRepository repository) =>
        _repository = repository;

    public async Task<Result<WaveResponse>> Handle(
        CreateWaveCommand request,
        CancellationToken cancellationToken)
    {
        var wave = new Wave
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            WaveDate = request.WaveDate
        };

        await _repository.CreateAsync(wave, cancellationToken);

        return Result<WaveResponse>.Success(new WaveResponse(wave.Id, wave.Name, wave.WaveDate));
    }
}
