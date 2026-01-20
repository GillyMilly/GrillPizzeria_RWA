using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class KorisnikSignInVM
{
    [Required(ErrorMessage = "Email ili Username je obavezan.")]
    [Display(Name = "Email ili Username")]
    public string EmailOrUsername { get; set; } = null!;

    [Required(ErrorMessage = "Lozinka je obavezna.")]
    [DataType(DataType.Password)]
    [Display(Name = "Lozinka")]
    public string Password { get; set; } = null!;

    [Display(Name = "Zapamti me")]
    public bool RememberMe { get; set; }
}
