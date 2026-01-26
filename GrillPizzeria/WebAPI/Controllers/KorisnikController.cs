using ClassLibrary.Models;
using ClassLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTO;
using WebAPI.Security;

namespace WebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class KorisnikController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly GrillPizzeriaDbContext _context;
    private readonly KorisnikRepository _korisnikRepository;

    public KorisnikController(IConfiguration configuration, GrillPizzeriaDbContext context, KorisnikRepository korisnikRepository)
    {
        _configuration = configuration;
        _context = context;
        _korisnikRepository = korisnikRepository;
    }

    [HttpPost("register")]
    public async Task<ActionResult<KorisnikRegisterDto>> Register([FromBody] KorisnikRegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var trimmedUsername = registerDto.Username.Trim();
            var existingUser = await _korisnikRepository.GetByUsernameAsync(trimmedUsername);
            if (existingUser != null)
                return BadRequest($"Username {trimmedUsername} već postoji.");

            var existingEmail = await _korisnikRepository.GetByEmailAsync(registerDto.Email);
            if (existingEmail != null)
                return BadRequest($"Email {registerDto.Email} već postoji.");

            var userRole = await _context.Roles.FirstOrDefaultAsync(x => x.RolesName == "User");
            if (userRole == null)
                return BadRequest("Uloga 'User' nije pronađena u bazi.");

            var b64salt = PasswordHashProvider.GetSalt();
            var b64hash = PasswordHashProvider.GetHash(registerDto.Password, b64salt);

            var user = new Korisnik
            {
                Username = trimmedUsername,
                PwdHash = b64hash,
                Salt = b64salt,
                Ime = registerDto.Ime,
                Prezime = registerDto.Prezime,
                Email = registerDto.Email,
                Mobitel = registerDto.Mobitel,
                RolesId = userRole.RolesId
            };

            await _korisnikRepository.AddKorisnikAsync(user);

            return Ok(new { message = "Korisnik je uspješno registriran.", id = user.Idkorisnik });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] KorisnikSignInDto signInDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var genericLoginFail = "Neispravno korisničko ime ili lozinka.";

            var existingUser = await _context.Korisniks
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Username == signInDto.EmailOrUsername || x.Email == signInDto.EmailOrUsername);

            if (existingUser == null)
                return BadRequest(genericLoginFail);

            var b64hash = PasswordHashProvider.GetHash(signInDto.Password, existingUser.Salt);
            if (b64hash != existingUser.PwdHash)
                return BadRequest(genericLoginFail);

            var secureKey = _configuration["JWT:SecureKey"] ?? "12345678901234567890123456789012";

            var serializedToken = JwtTokenProvider.CreateToken(
                secureKey,
                120,
                existingUser.Username,
                existingUser.Roles?.RolesName ?? "User");

            return Ok(new
            {
                token = serializedToken,
                username = existingUser.Username,
                role = existingUser.Roles?.RolesName ?? "User",
                userId = existingUser.Idkorisnik
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("change-role")]
    public async Task<ActionResult> ChangeRole([FromBody] KorisnikPromoteDto promoteDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RolesId == promoteDto.RolesId);
            if (role == null)
                return BadRequest($"Uloga s ID {promoteDto.RolesId} nije pronađena.");

            var korisnik = await _korisnikRepository.GetByUsernameAsync(promoteDto.Username);
            if (korisnik == null)
                return NotFound($"Korisnik s username '{promoteDto.Username}' nije pronađen.");

            korisnik.RolesId = promoteDto.RolesId;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Uloga korisnika '{promoteDto.Username}' je uspješno promijenjena na '{role.RolesName}'.",
                username = promoteDto.Username,
                role = role.RolesName
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
