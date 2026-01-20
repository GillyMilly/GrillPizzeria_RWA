using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class KorisnikChangePasswordVM
{
    [Required(ErrorMessage = "Trenutna lozinka je obavezna.")]
    [DataType(DataType.Password)]
    [Display(Name = "Trenutna lozinka")]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "Nova lozinka je obavezna.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Nova lozinka mora imati između 6 i 100 znakova.")]
    [DataType(DataType.Password)]
    [Display(Name = "Nova lozinka")]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = "Potvrda nove lozinke je obavezna.")]
    [Compare("NewPassword", ErrorMessage = "Nova lozinka i potvrda se ne podudaraju.")]
    [DataType(DataType.Password)]
    [Display(Name = "Potvrdi novu lozinku")]
    public string ConfirmNewPassword { get; set; } = null!;
}
