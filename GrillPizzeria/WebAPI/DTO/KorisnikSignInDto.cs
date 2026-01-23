using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class KorisnikSignInDto
{
    [Required(ErrorMessage = "Email ili Username je obavezan.")]
    public string EmailOrUsername { get; set; } = null!;

    [Required(ErrorMessage = "Lozinka je obavezna.")]
    public string Password { get; set; } = null!;
}
