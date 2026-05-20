using Microsoft.Data.Sqlite;


namespace ApiRefactor.Repositories;

public class WaveRepository : IWaveRepository
{
    private readonly string _connectionString;

    public WaveRepository(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("WaveContext")!;
    }

    public async Task<IEnumerable<Wave>> GetAllAsync()
    {
        var waves = new List<Wave>();

        using var connection =
            new SqliteConnection(_connectionString);

        await connection.OpenAsync();

        var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT id, name, wavedate
              FROM waves";

        using var reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            waves.Add(new Wave
            {
                Id = Guid.Parse(reader["id"].ToString()!),
                Name = reader["name"].ToString()!,
                WaveDate = DateTime.Parse(
                    reader["wavedate"].ToString()!)
            });
        }

        return waves;
    }

    public async Task<Wave?> GetByIdAsync(Guid id)
    {
        using var connection =
            new SqliteConnection(_connectionString);

        await connection.OpenAsync();

        var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT id, name, wavedate
              FROM waves
              WHERE id = @id";

        command.Parameters.AddWithValue("@id", id);

        using var reader =
            await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Wave
            {
                Id = Guid.Parse(reader["id"].ToString()!),
                Name = reader["name"].ToString()!,
                WaveDate = DateTime.Parse(
                    reader["wavedate"].ToString()!)
            };
        }

        return null;
    }

    public async Task SaveAsync(Wave wave)
    {
        using var connection =
            new SqliteConnection(_connectionString);

        await connection.OpenAsync();

        var existsCommand = connection.CreateCommand();

        existsCommand.CommandText =
            @"SELECT COUNT(1)
              FROM waves
              WHERE id = @id";

        existsCommand.Parameters.AddWithValue(
            "@id",
            wave.Id);

        var exists =
            Convert.ToInt32(
                await existsCommand.ExecuteScalarAsync()) > 0;

        var command = connection.CreateCommand();

        if (!exists)
        {
            command.CommandText =
                @"INSERT INTO waves
                    (id, name, wavedate)
                  VALUES
                    (@id, @name, @wavedate)";
        }
        else
        {
            command.CommandText =
                @"UPDATE waves
                  SET
                    name = @name,
                    wavedate = @wavedate
                  WHERE id = @id";
        }

        command.Parameters.AddWithValue("@id", wave.Id);
        command.Parameters.AddWithValue("@name", wave.Name);
        command.Parameters.AddWithValue(
            "@wavedate",
            wave.WaveDate);

        await command.ExecuteNonQueryAsync();
    }
}