using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class KategorijaHraneVM
{
    public int IdkategorijaHrane { get; set; }

    [Required(ErrorMessage = "Naziv kategorije je obavezan.")]
    [StringLength(100, ErrorMessage = "Naziv ne smije imati više od 100 znakova.")]
    [Display(Name = "Naziv kategorije")]
    public string Naziv { get; set; } = null!;

    [StringLength(255, ErrorMessage = "Opis ne smije imati više od 255 znakova.")]
    [Display(Name = "Opis")]
    public string? Opis { get; set; }
}
