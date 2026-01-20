using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class KorisnikRegisterVM
{
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

    [Required(ErrorMessage = "Lozinka je obavezna.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Lozinka mora imati između 6 i 100 znakova.")]
    [DataType(DataType.Password)]
    [Display(Name = "Lozinka")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Potvrda lozinke je obavezna.")]
    [Compare("Password", ErrorMessage = "Lozinke se ne podudaraju.")]
    [DataType(DataType.Password)]
    [Display(Name = "Potvrdi lozinku")]
    public string ConfirmPassword { get; set; } = null!;

    [StringLength(255, ErrorMessage = "Broj mobitela ne smije imati više od 255 znakova.")]
    [Phone(ErrorMessage = "Upišite ispravan broj mobitela.")]
    [Display(Name = "Mobitel (opcionalno)")]
    public string? Mobitel { get; set; }
}
