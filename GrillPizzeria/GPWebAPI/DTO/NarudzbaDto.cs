using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class NarudzbaDto
{
    public int Idnarudzba { get; set; }

    [Required(ErrorMessage = "Datum je obavezan.")]
    public DateTime Datum { get; set; }

    public int? KorisnikId { get; set; }
    public string? KorisnikIme { get; set; }
    public string? KorisnikEmail { get; set; }
    public List<NarudzbaHranaDto>? NarudzbaHranas { get; set; }
    public decimal? UkupnaCijena { get; set; }
}
