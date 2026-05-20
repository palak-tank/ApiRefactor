using System.ComponentModel.DataAnnotations;

public class Wave
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    public DateTime WaveDate { get; set; }
}