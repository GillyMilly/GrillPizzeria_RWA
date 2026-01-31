using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

/// <summary>
/// View model za AJAX ažuriranje profila (Ishod 5).
/// </summary>
public class KorisnikProfileUpdateVM
{
    [Required(ErrorMessage = "Email je obavezan.")]
    [EmailAddress(ErrorMessage = "Nevažeća email adresa.")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Ime je obavezno.")]
    [StringLength(100)]
    public string Ime { get; set; } = null!;

    [Required(ErrorMessage = "Prezime je obavezno.")]
    [StringLength(100)]
    public string Prezime { get; set; } = null!;

    [StringLength(255)]
    [Phone(ErrorMessage = "Upišite ispravan broj mobitela.")]
    public string? Mobitel { get; set; }
}
