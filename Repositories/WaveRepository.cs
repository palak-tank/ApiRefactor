using ApiRefactor.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ApiRefactor.Repositories;

public class WaveRepository : IWaveRepository
{
    private readonly string _connectionString;

    public WaveRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("WaveContext")!;
    }

    public async Task<IEnumerable<Wave>> GetAllAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        return await connection.QueryAsync<Wave>(
            "SELECT id, name, wavedate FROM waves");
    }

    public async Task<Wave?> GetByIdAsync(Guid id)
    {
        using var connection = new SqliteConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Wave>(
            "SELECT id, name, wavedate FROM waves WHERE id = @id",
            new { id });
    }

    public async Task CreateAsync(Wave wave)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.ExecuteAsync(
            "INSERT INTO waves (id, name, wavedate) VALUES (@Id, @Name, @WaveDate)",
            wave);
    }
}
