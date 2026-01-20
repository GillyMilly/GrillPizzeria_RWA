using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class KorisnikPromoteDto
{
    [Required(ErrorMessage = "ID korisnika je obavezan.")]
    public int Idkorisnik { get; set; }

    [Required(ErrorMessage = "ID uloge je obavezan.")]
    [Range(1, 2, ErrorMessage = "Uloga mora biti 1 (User) ili 2 (Admin).")]
    public int RolesId { get; set; }
}
