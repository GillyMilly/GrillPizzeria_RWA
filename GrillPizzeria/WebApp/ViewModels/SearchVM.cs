using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class SearchVM
{
    [Display(Name = "Pretraži hranu")]
    [StringLength(100, ErrorMessage = "Pretraga ne smije imati više od 100 znakova.")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Kategorija")]
    public int? KategorijaId { get; set; }

    [Range(1, 100, ErrorMessage = "Broj rezultata po stranici mora biti između 1 i 100.")]
    [Display(Name = "Rezultata po stranici")]
    public int PageSize { get; set; } = 10;

    [Range(1, int.MaxValue, ErrorMessage = "Stranica mora biti veća od 0.")]
    [Display(Name = "Stranica")]
    public int Page { get; set; } = 1;
}
