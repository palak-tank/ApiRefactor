using ApiRefactor.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ApiRefactor.Repositories;

public sealed class WaveRepository : IWaveRepository
{
    private readonly string _connectionString;
    private readonly ILogger<WaveRepository> _logger;

    public WaveRepository(IConfiguration configuration, ILogger<WaveRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("WaveContext")!;
        _logger = logger;
    }

    public async Task<(IEnumerable<Wave> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, CancellationToken ct)
    {
        _logger.LogDebug("Querying waves page {Page} with page size {PageSize}", page, pageSize);

        const string sql = """
            SELECT COUNT(*) FROM waves;
            SELECT id, name, wavedate FROM waves ORDER BY wavedate DESC LIMIT @pageSize OFFSET @offset;
            """;

        var parameters = new { pageSize, offset = (page - 1) * pageSize };
        var command = new CommandDefinition(sql, parameters, cancellationToken: ct);

        using var connection = new SqliteConnection(_connectionString);
        using var multi = await connection.QueryMultipleAsync(command);

        var totalCount = await multi.ReadSingleAsync<int>();
        var items = await multi.ReadAsync<Wave>();

        return (items, totalCount);
    }

    public async Task<Wave?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        _logger.LogDebug("Querying wave by {WaveId}", id);

        var command = new CommandDefinition(
            "SELECT id, name, wavedate FROM waves WHERE id = @id",
            new { id },
            cancellationToken: ct);

        using var connection = new SqliteConnection(_connectionString);
        var wave = await connection.QueryFirstOrDefaultAsync<Wave>(command);

        if (wave is null)
            _logger.LogWarning("Wave {WaveId} not found", id);

        return wave;
    }

    public async Task CreateAsync(Wave wave, CancellationToken ct)
    {
        _logger.LogDebug("Inserting new wave {WaveId}", wave.Id);

        var command = new CommandDefinition(
            "INSERT INTO waves (id, name, wavedate) VALUES (@Id, @Name, @WaveDate)",
            wave,
            cancellationToken: ct);

        using var connection = new SqliteConnection(_connectionString);
        await connection.ExecuteAsync(command);

        _logger.LogInformation("Created wave {WaveId} with name {WaveName}", wave.Id, wave.Name);
    }
}
