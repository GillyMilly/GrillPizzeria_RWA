using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Repositories;

public class KategorijaHraneRepository : IKategorijaHraneRepository
{
    private readonly GrillPizzeriaDbContext _context;

    public KategorijaHraneRepository(GrillPizzeriaDbContext context)
    {
        _context = context;
    }

    public IEnumerable<KategorijaHrane> GetAll() => _context.KategorijaHranes.ToList();

    public KategorijaHrane? GetById(int id) => _context.KategorijaHranes.FirstOrDefault(c => c.IdkategorijaHrane == id);

    public void Add(KategorijaHrane kategorijaHrane)
    {
        _context.KategorijaHranes.Add(kategorijaHrane);
        Save();
    }

    public void Update(KategorijaHrane kategorijaHrane)
    {
        var existingCategory = _context.KategorijaHranes.FirstOrDefault(c => c.IdkategorijaHrane == kategorijaHrane.IdkategorijaHrane);
        if (existingCategory != null)
        {
            existingCategory.Naziv = kategorijaHrane.Naziv;
            existingCategory.Opis = kategorijaHrane.Opis;
            Save();
        }
    }

    public void Delete(int id)
    {
        var category = GetById(id);
        if (category != null)
        {
            _context.KategorijaHranes.Remove(category);
            Save();
        }
    }

    public void Save() => _context.SaveChanges();
}
