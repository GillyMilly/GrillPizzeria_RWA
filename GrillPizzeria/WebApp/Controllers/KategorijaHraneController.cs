using AutoMapper;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class KategorijaHraneController : Controller
{
    private readonly IKategorijaHraneRepository _repository;
    private readonly IMapper _mapper;

    public KategorijaHraneController(IKategorijaHraneRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public ActionResult Index()
    {
        try
        {
            var kategorije = _repository.GetAll();
            var kategorijaVms = _mapper.Map<List<KategorijaHraneVM>>(kategorije);
            return View(kategorijaVms);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    public ActionResult Details(int id)
    {
        try
        {
            var kategorija = _repository.GetById(id);
            if (kategorija == null)
                return NotFound();

            var kategorijaVM = _mapper.Map<KategorijaHraneVM>(kategorija);
            return View(kategorijaVM);
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
    public ActionResult Create(KategorijaHraneVM kategorijaVM)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(kategorijaVM);

            var newKategorija = _mapper.Map<KategorijaHrane>(kategorijaVM);
            _repository.Add(newKategorija);

            TempData["SuccessMessage"] = $"Kategorija '{newKategorija.Naziv}' je uspješno kreirana.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError("", "Greška pri kreiranju kategorije.");
            return View(kategorijaVM);
        }
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Edit(int id)
    {
        try
        {
            var kategorija = _repository.GetById(id);
            if (kategorija == null)
                return NotFound();

            var kategorijaVM = _mapper.Map<KategorijaHraneVM>(kategorija);
            return View(kategorijaVM);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, KategorijaHraneVM kategorijaVM)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(kategorijaVM);

            var existingKategorija = _repository.GetById(id);
            if (existingKategorija == null)
                return NotFound();

            _mapper.Map(kategorijaVM, existingKategorija);
            _repository.Update(existingKategorija);

            TempData["SuccessMessage"] = "Kategorija je uspješno ažurirana.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError("", "Greška pri ažuriranju kategorije.");
            return View(kategorijaVM);
        }
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        try
        {
            var kategorija = _repository.GetById(id);
            if (kategorija == null)
                return NotFound();

            var kategorijaVM = _mapper.Map<KategorijaHraneVM>(kategorija);
            return View(kategorijaVM);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, KategorijaHraneVM kategorijaVM)
    {
        try
        {
            var kategorija = _repository.GetById(id);
            if (kategorija == null)
                return NotFound();

            _repository.Delete(id);
            TempData["SuccessMessage"] = "Kategorija je uspješno obrisana.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError("", "Greška pri brisanju kategorije.");
            return View(kategorijaVM);
        }
    }
}
