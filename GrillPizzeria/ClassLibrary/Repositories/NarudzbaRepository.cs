using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Repositories;

public class NarudzbaRepository
{
    private readonly GrillPizzeriaDbContext _context;

    public NarudzbaRepository(GrillPizzeriaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Narudzba>> GetNarudzbeAsync()
    {
        return await _context.Narudzbas
            .Include(n => n.Korisnik)
            .Include(n => n.NarudzbaHranas)
                .ThenInclude(nh => nh.Hrana)
            .ToListAsync();
    }

    public async Task<Narudzba?> GetNarudzbaByIdAsync(int id)
    {
        return await _context.Narudzbas
            .Include(n => n.Korisnik)
            .Include(n => n.NarudzbaHranas)
                .ThenInclude(nh => nh.Hrana)
            .FirstOrDefaultAsync(n => n.Idnarudzba == id);
    }

    public async Task<List<Narudzba>> GetNarudzbeByKorisnikIdAsync(int korisnikId)
    {
        return await _context.Narudzbas
            .Where(n => n.KorisnikId == korisnikId)
            .Include(n => n.Korisnik)
            .Include(n => n.NarudzbaHranas)
                .ThenInclude(nh => nh.Hrana)
            .ToListAsync();
    }

    public async Task AddNarudzbaAsync(Narudzba narudzba)
    {
        _context.Narudzbas.Add(narudzba);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNarudzbaAsync(Narudzba narudzba)
    {
        _context.Narudzbas.Update(narudzba);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNarudzbaAsync(int id)
    {
        var narudzba = await _context.Narudzbas.FindAsync(id);
        if (narudzba != null)
        {
            _context.Narudzbas.Remove(narudzba);
            await _context.SaveChangesAsync();
        }
    }
}
