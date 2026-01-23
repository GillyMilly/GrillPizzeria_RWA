using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class HranaDto
{
    public int Idhrana { get; set; }

    [Required(ErrorMessage = "Naslov hrane je obavezan.")]
    [StringLength(100, ErrorMessage = "Naslov ne smije imati više od 100 znakova.")]
    public string Naslov { get; set; } = null!;

    [StringLength(255, ErrorMessage = "Opis ne smije imati više od 255 znakova.")]
    public string? Opis { get; set; }

    [Range(0.01, 9999.99, ErrorMessage = "Cijena mora biti između 0.01 i 9999.99.")]
    [DisplayFormat(DataFormatString = "{0:F2}")]
    public decimal? Cijena { get; set; }

    public int? KategorijaHraneId { get; set; }
    public string? KategorijaNaziv { get; set; }
    public List<int>? AlergenIds { get; set; }
    public List<string>? AlergenNazivi { get; set; }
}
