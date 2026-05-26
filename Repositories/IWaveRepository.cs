using ApiRefactor.Models;

namespace ApiRefactor.Repositories;

public interface IWaveRepository
{
    Task<IEnumerable<Wave>> GetAllAsync();
    Task<Wave?> GetByIdAsync(Guid id);
    Task CreateAsync(Wave wave);
}
