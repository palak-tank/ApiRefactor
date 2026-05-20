public interface IWaveRepository
{
  public Task<IEnumerable<Wave>> GetAllAsync();
  public Task<Wave?> GetByIdAsync(Guid id);
  public Task SaveAsync(Wave wave);
}