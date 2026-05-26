using ApiRefactor.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ApiRefactor.Repositories;

public class WaveRepository : IWaveRepository
{
    private readonly string _connectionString;
    private readonly ILogger<WaveRepository> _logger;

    public WaveRepository(IConfiguration configuration, ILogger<WaveRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("WaveContext")!;
        _logger = logger;
    }

    public async Task<IEnumerable<Wave>> GetAllAsync()
    {
        _logger.LogDebug("Querying all waves");

        using var connection = new SqliteConnection(_connectionString);
        return await connection.QueryAsync<Wave>(
            "SELECT id, name, wavedate FROM waves");
    }

    public async Task<Wave?> GetByIdAsync(Guid id)
    {
        _logger.LogDebug("Querying wave by {WaveId}", id);

        using var connection = new SqliteConnection(_connectionString);
        var wave = await connection.QueryFirstOrDefaultAsync<Wave>(
            "SELECT id, name, wavedate FROM waves WHERE id = @id",
            new { id });

        if (wave is null)
            _logger.LogWarning("Wave {WaveId} not found", id);

        return wave;
    }

    public async Task CreateAsync(Wave wave)
    {
        _logger.LogDebug("Inserting new wave {WaveId}", wave.Id);

        using var connection = new SqliteConnection(_connectionString);
        await connection.ExecuteAsync(
            "INSERT INTO waves (id, name, wavedate) VALUES (@Id, @Name, @WaveDate)",
            wave);

        _logger.LogInformation("Created wave {WaveId} with name {WaveName}", wave.Id, wave.Name);
    }
}
