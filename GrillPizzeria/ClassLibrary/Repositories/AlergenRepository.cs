using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Repositories;

public class AlergenRepository : IAlergenRepository
{
    private readonly GrillPizzeriaDbContext _context;

    public AlergenRepository(GrillPizzeriaDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Alergen> GetAll() => _context.Alergens.ToList();

    public Alergen? GetById(int id) => _context.Alergens.FirstOrDefault(a => a.Idalergen == id);

    public void Add(Alergen alergen)
    {
        _context.Alergens.Add(alergen);
        Save();
    }

    public void Update(Alergen alergen)
    {
        var existingAlergen = _context.Alergens.FirstOrDefault(a => a.Idalergen == alergen.Idalergen);
        if (existingAlergen != null)
        {
            existingAlergen.Naziv = alergen.Naziv;
            Save();
        }
    }

    public void Delete(int id)
    {
        var alergen = GetById(id);
        if (alergen != null)
        {
            _context.Alergens.Remove(alergen);
            Save();
        }
    }

    public void Save() => _context.SaveChanges();
}
