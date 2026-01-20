using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class AlergenDto
{
    public int Idalergen { get; set; }

    [Required(ErrorMessage = "Naziv alergena je obavezan.")]
    [StringLength(100, ErrorMessage = "Naziv alergena ne smije imati više od 100 znakova.")]
    public string Naziv { get; set; } = null!;
}
