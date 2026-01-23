using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO;

public class NarudzbaHranaDto
{
    public int IdnarudzbaHrana { get; set; }

    [Required(ErrorMessage = "ID narudžbe je obavezan.")]
    public int NarudzbaId { get; set; }

    [Required(ErrorMessage = "ID hrane je obavezan.")]
    public int HranaId { get; set; }

    [Required(ErrorMessage = "Količina je obavezna.")]
    [Range(1, 100, ErrorMessage = "Količina mora biti između 1 i 100.")]
    public int Kolicina { get; set; }

    public string? HranaNaslov { get; set; }
    public decimal? HranaCijena { get; set; }
    public decimal? UkupnaCijena { get; set; }
}
