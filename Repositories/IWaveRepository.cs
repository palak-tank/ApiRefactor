using ApiRefactor.DTOs;
using ApiRefactor.Models;

namespace ApiRefactor.Repositories;

public interface IWaveRepository
{
    Task<(IEnumerable<Wave> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken ct);
    Task<Wave?> GetByIdAsync(Guid id, CancellationToken ct);
    Task CreateAsync(Wave wave, CancellationToken ct);
}
