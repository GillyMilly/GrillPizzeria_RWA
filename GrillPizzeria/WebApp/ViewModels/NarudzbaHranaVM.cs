using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class NarudzbaHranaVM
{
    public int IdnarudzbaHrana { get; set; }

    [Required(ErrorMessage = "ID narudžbe je obavezan.")]
    public int NarudzbaId { get; set; }

    [Required(ErrorMessage = "ID hrane je obavezan.")]
    [Display(Name = "Hrana")]
    public int HranaId { get; set; }

    [Required(ErrorMessage = "Količina je obavezna.")]
    [Range(1, 100, ErrorMessage = "Količina mora biti između 1 i 100.")]
    [Display(Name = "Količina")]
    public int Kolicina { get; set; }

    [Display(Name = "Naziv hrane")]
    public string? HranaNaslov { get; set; }

    [Display(Name = "Cijena po komadu")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal? HranaCijena { get; set; }

    [Display(Name = "Ukupna cijena")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal? UkupnaCijena { get; set; }
}
