using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class HranaVM
{
    public int Idhrana { get; set; }

    [Required(ErrorMessage = "Naslov hrane je obavezan.")]
    [StringLength(100, ErrorMessage = "Naslov ne smije imati više od 100 znakova.")]
    [Display(Name = "Naslov")]
    public string Naslov { get; set; } = null!;

    [StringLength(255, ErrorMessage = "Opis ne smije imati više od 255 znakova.")]
    [Display(Name = "Opis")]
    public string? Opis { get; set; }

    [Range(0.01, 9999.99, ErrorMessage = "Cijena mora biti između 0.01 i 9999.99.")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    [Display(Name = "Cijena")]
    public decimal? Cijena { get; set; }

    [Display(Name = "Kategorija hrane")]
    public int? KategorijaHraneId { get; set; }
    public string? KategorijaNaziv { get; set; }

    [Display(Name = "Odabrani alergeni")]
    public List<int> OdabraniAlergeni { get; set; } = new List<int>();

    [Display(Name = "Dostupni alergeni")]
    public List<SelectListItem>? AlergeniDdl { get; set; }

    [Display(Name = "Alergeni")]
    public List<string>? AlergeniNazivi { get; set; }

    [Display(Name = "Slika")]
    public string? SlikaUrl { get; set; }
}
