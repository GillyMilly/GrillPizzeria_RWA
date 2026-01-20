using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class LogVM
{
    public int Id { get; set; }

    [Display(Name = "Vrijeme")]
    public DateTime? Timestamp { get; set; }

    [Display(Name = "Razina")]
    public string Level { get; set; } = null!;

    [Display(Name = "Poruka")]
    public string Message { get; set; } = null!;
}
