using AutoMapper;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class AlergenController : Controller
{
    private readonly IAlergenRepository _repository;
    private readonly IMapper _mapper;

    public AlergenController(IAlergenRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [Authorize]
    public ActionResult Index()
    {
        try
        {
            var alergens = _repository.GetAll();
            var alergenVms = _mapper.Map<List<AlergenVM>>(alergens);
            return View(alergenVms);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [Authorize]
    public ActionResult Details(int id)
    {
        try
        {
            var alergen = _repository.GetById(id);
            if (alergen == null)
                return NotFound();

            var alergenVM = _mapper.Map<AlergenVM>(alergen);
            return View(alergenVM);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(AlergenVM alergenVM)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(alergenVM);

            var newAlergen = _mapper.Map<Alergen>(alergenVM);
            _repository.Add(newAlergen);

            TempData["SuccessMessage"] = $"Alergen '{newAlergen.Naziv}' je uspješno kreiran.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError("", "Greška pri kreiranju alergena.");
            return View(alergenVM);
        }
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Edit(int id)
    {
        try
        {
            var alergen = _repository.GetById(id);
            if (alergen == null)
                return NotFound();

            var alergenVM = _mapper.Map<AlergenVM>(alergen);
            return View(alergenVM);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, AlergenVM alergenVM)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(alergenVM);

            var existingAlergen = _repository.GetById(id);
            if (existingAlergen == null)
                return NotFound();

            _mapper.Map(alergenVM, existingAlergen);
            _repository.Update(existingAlergen);

            TempData["SuccessMessage"] = "Alergen je uspješno ažuriran.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError("", "Greška pri ažuriranju alergena.");
            return View(alergenVM);
        }
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        try
        {
            var alergen = _repository.GetById(id);
            if (alergen == null)
                return NotFound();

            var alergenVM = _mapper.Map<AlergenVM>(alergen);
            return View(alergenVM);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, AlergenVM alergenVM)
    {
        try
        {
            var alergen = _repository.GetById(id);
            if (alergen == null)
                return NotFound();

            _repository.Delete(id);
            TempData["SuccessMessage"] = "Alergen je uspješno obrisan.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError("", "Greška pri brisanju alergena.");
            return View(alergenVM);
        }
    }
}
