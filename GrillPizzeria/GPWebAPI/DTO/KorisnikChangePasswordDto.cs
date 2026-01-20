using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class KorisnikChangePasswordDto
{
    [Required(ErrorMessage = "Trenutna lozinka je obavezna.")]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "Nova lozinka je obavezna.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Nova lozinka mora imati između 6 i 100 znakova.")]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = "Potvrda nove lozinke je obavezna.")]
    [Compare("NewPassword", ErrorMessage = "Nova lozinka i potvrda se ne podudaraju.")]
    public string ConfirmNewPassword { get; set; } = null!;
}
