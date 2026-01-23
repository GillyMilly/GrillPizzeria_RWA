using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class LogDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Level je obavezan.")]
    [StringLength(20, ErrorMessage = "Level ne smije imati više od 20 znakova.")]
    public string Level { get; set; } = null!;

    [Required(ErrorMessage = "Message je obavezan.")]
    [StringLength(500, ErrorMessage = "Message ne smije imati više od 500 znakova.")]
    public string Message { get; set; } = null!;

    public DateTime? Timestamp { get; set; }
}
