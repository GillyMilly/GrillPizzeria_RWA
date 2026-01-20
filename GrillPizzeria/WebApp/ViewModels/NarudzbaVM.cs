using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class NarudzbaVM
{
    public int Idnarudzba { get; set; }

    [Required(ErrorMessage = "Datum je obavezan.")]
    [Display(Name = "Datum narudžbe")]
    [DataType(DataType.DateTime)]
    public DateTime Datum { get; set; }

    [Display(Name = "Korisnik")]
    public int? KorisnikId { get; set; }
    public string? KorisnikIme { get; set; }
    public string? KorisnikEmail { get; set; }

    [Display(Name = "Stavke narudžbe")]
    public List<NarudzbaHranaVM>? NarudzbaHranas { get; set; }

    [Display(Name = "Ukupna cijena")]
    [DisplayFormat(DataFormatString = "{0:F2} HRK")]
    public decimal? UkupnaCijena { get; set; }
}
