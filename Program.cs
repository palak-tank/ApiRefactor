using ApiRefactor.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/wave", () => new Waves())
    .WithName("GetWaves")
    .WithOpenApi();

app.MapGet("/api/wave/{id}", (Guid id) => new Wave(id))
    .WithName("GetWaveById")
    .WithOpenApi();

app.MapPost("/api/wave", (Wave wave) => { wave.Save(); })
    .WithName("UpsertWave")
    .WithOpenApi();

app.Run();
