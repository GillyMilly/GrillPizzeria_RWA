using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Repositories;

public class HranaRepository
{
    private readonly GrillPizzeriaDbContext _context;

    public HranaRepository(GrillPizzeriaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Hrana>> GetAllHranaAsync()
    {
        return await _context.Hranas
            .Include(h => h.KategorijaHrane)
            .Include(h => h.HranaAlergens)
                .ThenInclude(ha => ha.Alergen)
            .ToListAsync();
    }

    public async Task<Hrana?> GetHranaByIdAsync(int id)
    {
        return await _context.Hranas
            .Include(h => h.KategorijaHrane)
            .Include(h => h.HranaAlergens)
                .ThenInclude(ha => ha.Alergen)
            .FirstOrDefaultAsync(h => h.Idhrana == id);
    }

    public async Task AddHranaAsync(Hrana hrana)
    {
        _context.Hranas.Add(hrana);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateHranaAsync(Hrana hrana)
    {
        _context.Hranas.Update(hrana);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteHranaAsync(int id)
    {
        var hrana = await _context.Hranas.FindAsync(id);
        if (hrana != null)
        {
            _context.Hranas.Remove(hrana);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Hrana>> SearchAsync(string name, int page, int count)
    {
        return await _context.Hranas
            .Where(h => h.Naslov.Contains(name) || h.Opis != null && h.Opis.Contains(name))
            .Skip((page - 1) * count)
            .Take(count)
            .ToListAsync();
    }

    public async Task<(List<Hrana> Items, int TotalCount)> SearchWithPagingAsync(string? name, int page, int count)
    {
        var query = _context.Hranas
            .Include(h => h.KategorijaHrane)
            .Include(h => h.HranaAlergens)
                .ThenInclude(ha => ha.Alergen)
            .AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(h => h.Naslov.Contains(name));
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(h => h.Naslov)
            .Skip((page - 1) * count)
            .Take(count)
            .Include(h => h.KategorijaHrane)
            .ToListAsync();

        return (items, total);
    }
}
