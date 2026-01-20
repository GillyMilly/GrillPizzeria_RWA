using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class KorisnikDto
{
    public int Idkorisnik { get; set; }

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

    [StringLength(50, ErrorMessage = "Username ne smije imati više od 50 znakova.")]
    public string? Username { get; set; }

    public int RolesId { get; set; }
    public string? RoleName { get; set; }
}
