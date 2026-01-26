using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Repositories;

public class KorisnikRepository
{
    private readonly GrillPizzeriaDbContext _context;

    public KorisnikRepository(GrillPizzeriaDbContext context)
    {
        _context = context;
    }

    public async Task<Korisnik?> GetByEmailAsync(string email)
    {
        return await _context.Korisniks.FirstOrDefaultAsync(k => k.Email == email);
    }

    public async Task<Korisnik?> GetByUsernameAsync(string username)
    {
        return await _context.Korisniks.FirstOrDefaultAsync(k => k.Username == username);
    }

    public async Task<Korisnik?> GetByIdAsync(int id)
    {
        return await _context.Korisniks
            .Include(k => k.Roles)
            .FirstOrDefaultAsync(k => k.Idkorisnik == id);
    }

    public async Task<List<Korisnik>> GetAllAsync()
    {
        return await _context.Korisniks
            .Include(k => k.Roles)
            .ToListAsync();
    }

    public async Task AddKorisnikAsync(Korisnik korisnik)
    {
        _context.Korisniks.Add(korisnik);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateKorisnikAsync(Korisnik korisnik)
    {
        var existingKorisnik = await _context.Korisniks
            .FirstOrDefaultAsync(k => k.Idkorisnik == korisnik.Idkorisnik);
        
        if (existingKorisnik == null)
            throw new InvalidOperationException($"Korisnik s ID {korisnik.Idkorisnik} nije pronađen.");

        // Update only the properties that should be changed
        existingKorisnik.Ime = korisnik.Ime;
        existingKorisnik.Prezime = korisnik.Prezime;
        existingKorisnik.Email = korisnik.Email;
        existingKorisnik.Mobitel = korisnik.Mobitel;
        // Username, PwdHash, Salt, and RolesId should not be changed here
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteKorisnikAsync(int id)
    {
        var korisnik = await _context.Korisniks.FindAsync(id);
        if (korisnik != null)
        {
            _context.Korisniks.Remove(korisnik);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateKorisnikRoleAsync(string username, int roleId)
    {
        var korisnik = await _context.Korisniks
            .FirstOrDefaultAsync(k => k.Username == username);
        
        if (korisnik == null)
            throw new InvalidOperationException($"Korisnik s username '{username}' nije pronađen.");

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RolesId == roleId);
        if (role == null)
            throw new InvalidOperationException($"Uloga s ID {roleId} nije pronađena.");

        korisnik.RolesId = roleId;
        await _context.SaveChangesAsync();
    }
}
