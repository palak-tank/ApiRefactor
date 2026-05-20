using ApiRefactor.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWaveRepository, WaveRepository>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/wave", async (IWaveRepository waveRepository) =>
{
     var waves = await waveRepository.GetAllAsync();
     return Results.Ok(waves);
})
    .WithName("GetWaves")
    .WithOpenApi();

app.MapGet("/api/wave/{id:guid}", 
    async (Guid id, IWaveRepository waveRepository) => {
        var wave = await waveRepository.GetByIdAsync(id);
        return wave is null
            ? Results.NotFound()
            : Results.Ok(wave);
        })
    .WithName("GetWaveById")
    .WithOpenApi();

app.MapPost("/api/wave", async (Wave wave, IWaveRepository waveRepository) => { 
    var waveObj = new Wave
        {
            Id = Guid.NewGuid(),
            Name = wave.Name,
            WaveDate = wave.WaveDate
        };
    await waveRepository.SaveAsync(waveObj);
     return Results.Created(
            $"/api/wave/{waveObj.Id}",
            waveObj); 
    })
    .WithName("UpsertWave")
    .WithOpenApi();

app.Run();
