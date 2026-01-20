using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class KorisnikPromoteVM
{
    [Required(ErrorMessage = "ID korisnika je obavezan.")]
    public int Idkorisnik { get; set; }

    [Required(ErrorMessage = "Uloga je obavezna.")]
    [Range(1, 2, ErrorMessage = "Uloga mora biti 1 (User) ili 2 (Admin).")]
    [Display(Name = "Uloga")]
    public int RolesId { get; set; }

    [Display(Name = "Korisnik")]
    public string? KorisnikIme { get; set; }
}
