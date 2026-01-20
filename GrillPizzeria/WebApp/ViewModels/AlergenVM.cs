using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class AlergenVM
{
    public int Idalergen { get; set; }

    [Required(ErrorMessage = "Naziv alergena je obavezan.")]
    [StringLength(100, ErrorMessage = "Naziv alergena ne smije imati više od 100 znakova.")]
    [Display(Name = "Naziv alergena")]
    public string Naziv { get; set; } = null!;
}
