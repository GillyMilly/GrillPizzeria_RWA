using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class KategorijaHraneDto
{
    public int IdkategorijaHrane { get; set; }

    [Required(ErrorMessage = "Naziv kategorije je obavezan.")]
    [StringLength(100, ErrorMessage = "Naziv ne smije imati više od 100 znakova.")]
    public string Naziv { get; set; } = null!;

    [StringLength(255, ErrorMessage = "Opis ne smije imati više od 255 znakova.")]
    public string? Opis { get; set; }
}
