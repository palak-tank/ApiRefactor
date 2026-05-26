using System.ComponentModel.DataAnnotations;

namespace ApiRefactor.DTOs;

public sealed class CreateWaveRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    public DateTime WaveDate { get; init; }
}
