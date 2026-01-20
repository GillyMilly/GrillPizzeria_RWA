using AutoMapper;
using ClassLibrary.Models;
using ClassLibrary.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.Models;
using WebApp.Security;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class KorisnikController : Controller
{
    private readonly IMapper _mapper;
    private readonly GrillPizzeriaDbContext _context;
    private readonly KorisnikRepository _korisnikRepository;

    public KorisnikController(GrillPizzeriaDbContext context, IMapper mapper, KorisnikRepository korisnikRepository)
    {
        _context = context;
        _mapper = mapper;
        _korisnikRepository = korisnikRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult SignIn(string? returnUrl)
    {
        var signInVm = new KorisnikSignInVM
        {
            ReturnUrl = returnUrl
        };
        return View(signInVm);
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(KorisnikSignInVM signInVm)
    {
        if (!ModelState.IsValid)
            return View(signInVm);

        var existingUser = await _context.Korisniks
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Username == signInVm.EmailOrUsername || x.Email == signInVm.EmailOrUsername);

        if (existingUser == null)
        {
            ModelState.AddModelError("", "Nevažeće korisničko ime ili lozinka.");
            return View(signInVm);
        }

        var b64hash = PasswordHashProvider.GetHash(signInVm.Password, existingUser.Salt);
        if (b64hash != existingUser.PwdHash)
        {
            ModelState.AddModelError("", "Nevažeće korisničko ime ili lozinka.");
            return View(signInVm);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, existingUser.Username),
            new Claim(ClaimTypes.Role, existingUser.Roles?.RolesName ?? "User")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties();

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        if (!string.IsNullOrEmpty(signInVm.ReturnUrl))
            return LocalRedirect(signInVm.ReturnUrl);
        else if (existingUser.Roles?.RolesName == "Admin")
            return RedirectToAction("Index", "Home");
        else
            return RedirectToAction("Index", "Home");
    }

    public new async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(KorisnikRegisterVM registerVm)
    {
        if (!ModelState.IsValid)
            return View(registerVm);

        try
        {
            var trimmedUsername = registerVm.Username.Trim();
            var existingUser = await _korisnikRepository.GetByUsernameAsync(trimmedUsername);
            if (existingUser != null)
            {
                ModelState.AddModelError("", $"Korisničko ime {trimmedUsername} već postoji!");
                return View(registerVm);
            }

            var existingEmail = await _korisnikRepository.GetByEmailAsync(registerVm.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("", $"Email {registerVm.Email} već postoji!");
                return View(registerVm);
            }

            var userRole = await _context.Roles.FirstOrDefaultAsync(x => x.RolesName == "User");
            if (userRole == null)
            {
                ModelState.AddModelError("", "Uloga 'User' nije pronađena u bazi.");
                return View(registerVm);
            }

            var b64salt = PasswordHashProvider.GetSalt();
            var b64hash = PasswordHashProvider.GetHash(registerVm.Password, b64salt);

            var korisnik = new Korisnik
            {
                Username = trimmedUsername,
                PwdHash = b64hash,
                Salt = b64salt,
                Ime = registerVm.Ime,
                Prezime = registerVm.Prezime,
                Email = registerVm.Email,
                Mobitel = registerVm.Mobitel,
                RolesId = userRole.RolesId
            };

            await _korisnikRepository.AddKorisnikAsync(korisnik);

            TempData["SuccessMessage"] = "Registracija je uspješna! Možete se prijaviti.";
            return RedirectToAction("SignIn");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Greška pri registraciji: {ex.Message}");
            return View(registerVm);
        }
    }

    [Authorize]
    public async Task<IActionResult> KorisnikDetails()
    {
        try
        {
            var username = HttpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("SignIn");

            var userdb = await _korisnikRepository.GetByUsernameAsync(username);
            if (userdb == null)
                return NotFound();

            var userVm = _mapper.Map<KorisnikVM>(userdb);
            return View(userVm);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [Authorize]
    public async Task<IActionResult> KorisnikEdit(int id)
    {
        try
        {
            var userDb = await _korisnikRepository.GetByIdAsync(id);
            if (userDb == null)
                return NotFound();

            var userVm = _mapper.Map<KorisnikVM>(userDb);
            return View(userVm);
        }
        catch (Exception)
        {
            return View("Error", new ErrorViewModel { RequestId = "Greška pri učitavanju korisnika." });
        }
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> KorisnikEdit(int id, KorisnikVM userVm)
    {
        if (!ModelState.IsValid)
            return View(userVm);

        try
        {
            var userDb = await _korisnikRepository.GetByIdAsync(id);
            if (userDb == null)
                return NotFound();

            _mapper.Map(userVm, userDb);
            await _korisnikRepository.UpdateKorisnikAsync(userDb);

            TempData["SuccessMessage"] = "Profil je uspješno ažuriran.";
            return RedirectToAction("KorisnikDetails");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Greška pri ažuriranju: {ex.Message}");
            return View(userVm);
        }
    }

    [Authorize]
    public IActionResult Forbidden()
    {
        return View();
    }
}
