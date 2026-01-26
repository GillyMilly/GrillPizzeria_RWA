using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class KorisnikPromoteDto
{
    [Required(ErrorMessage = "Username korisnika je obavezan.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "ID uloge je obavezan.")]
    [Range(1, 2, ErrorMessage = "Uloga mora biti 1 (User) ili 2 (Admin).")]
    public int RolesId { get; set; }
}
