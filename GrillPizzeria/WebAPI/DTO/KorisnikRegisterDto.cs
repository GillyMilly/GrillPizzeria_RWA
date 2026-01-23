using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class KorisnikRegisterDto
{
    [Required(ErrorMessage = "Ime je obavezno.")]
    [StringLength(100, ErrorMessage = "Ime ne smije imati više od 100 znakova.")]
    public string Ime { get; set; } = null!;

    [Required(ErrorMessage = "Prezime je obavezno.")]
    [StringLength(100, ErrorMessage = "Prezime ne smije imati više od 100 znakova.")]
    public string Prezime { get; set; } = null!;

    [Required(ErrorMessage = "Email je obavezan.")]
    [EmailAddress(ErrorMessage = "Nevažeća email adresa.")]
    [StringLength(100, ErrorMessage = "Email ne smije imati više od 100 znakova.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Username je obavezan.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username mora imati između 3 i 50 znakova.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Lozinka je obavezna.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Lozinka mora imati između 6 i 100 znakova.")]
    public string Password { get; set; } = null!;

    [StringLength(255, ErrorMessage = "Broj mobitela ne smije imati više od 255 znakova.")]
    public string? Mobitel { get; set; }
}
