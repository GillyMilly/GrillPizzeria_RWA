using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class KorisnikVM
{
    public int Idkorisnik { get; set; }

    [Required(ErrorMessage = "Ime je obavezno.")]
    [StringLength(100, ErrorMessage = "Ime ne smije imati više od 100 znakova.")]
    [Display(Name = "Ime")]
    public string Ime { get; set; } = null!;

    [Required(ErrorMessage = "Prezime je obavezno.")]
    [StringLength(100, ErrorMessage = "Prezime ne smije imati više od 100 znakova.")]
    [Display(Name = "Prezime")]
    public string Prezime { get; set; } = null!;

    [Required(ErrorMessage = "Email je obavezan.")]
    [EmailAddress(ErrorMessage = "Nevažeća email adresa.")]
    [StringLength(100, ErrorMessage = "Email ne smije imati više od 100 znakova.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Username je obavezan.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username mora imati između 3 i 50 znakova.")]
    [Display(Name = "Korisničko ime")]
    public string Username { get; set; } = null!;

    [StringLength(255, ErrorMessage = "Broj mobitela ne smije imati više od 255 znakova.")]
    [Phone(ErrorMessage = "Upišite ispravan broj mobitela.")]
    [Display(Name = "Mobitel")]
    public string? Mobitel { get; set; }

    [Display(Name = "Uloga")]
    public int RolesId { get; set; }
    public string? RoleName { get; set; }
}
