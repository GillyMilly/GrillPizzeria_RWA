using AutoMapper;
using ClassLibrary.Models;
using ClassLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class HranaController : Controller
{
    private readonly HranaRepository _hranaRepository;
    private readonly IMapper _mapper;
    private readonly GrillPizzeriaDbContext _context;

    public HranaController(HranaRepository hranaRepository, IMapper mapper, GrillPizzeriaDbContext context)
    {
        _hranaRepository = hranaRepository;
        _mapper = mapper;
        _context = context;
    }

    private List<SelectListItem> GetCategoryListItems()
    {
        return _context.KategorijaHranes
            .Select(x => new SelectListItem
            {
                Text = x.Naziv,
                Value = x.IdkategorijaHrane.ToString()
            }).ToList();
    }

    private List<SelectListItem> GetAlergenListItems()
    {
        return _context.Alergens
            .Select(a => new SelectListItem
            {
                Text = a.Naziv,
                Value = a.Idalergen.ToString()
            }).ToList();
    }

    public async Task<ActionResult> Index()
    {
        try
        {
            var hranaList = await _hranaRepository.GetAllHranaAsync();
            var hranaVms = _mapper.Map<List<HranaVM>>(hranaList);
            return View(hranaVms);
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
            var hrana = _context.Hranas
                .Include(h => h.KategorijaHrane)
                .Include(h => h.HranaAlergens)
                    .ThenInclude(ha => ha.Alergen)
                .FirstOrDefault(h => h.Idhrana == id);

            if (hrana == null)
                return NotFound();

            var hranaVM = _mapper.Map<HranaVM>(hrana);
            hranaVM.AlergeniNazivi = hrana.HranaAlergens
                .Where(ha => ha.Alergen != null)
                .Select(ha => ha.Alergen.Naziv)
                .ToList();

            return View(hranaVM);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    public ActionResult Search(SearchVM searchVM)
    {
        try
        {
            if (searchVM.Page < 1) searchVM.Page = 1;
            if (searchVM.PageSize < 1) searchVM.PageSize = 10;

            IQueryable<Hrana> foodItems = _context.Hranas.Include(x => x.KategorijaHrane);

            if (!string.IsNullOrEmpty(searchVM.SearchTerm))
            {
                foodItems = foodItems.Where(x => x.Naslov.Contains(searchVM.SearchTerm));
            }

            if (searchVM.KategorijaId.HasValue)
            {
                foodItems = foodItems.Where(x => x.KategorijaHraneId == searchVM.KategorijaId.Value);
            }

            var filteredCount = foodItems.Count();
            var items = foodItems
                .OrderBy(x => x.Naslov)
                .Skip((searchVM.Page - 1) * searchVM.PageSize)
                .Take(searchVM.PageSize)
                .ToList();

            var hranaVms = _mapper.Map<List<HranaVM>>(items);
            searchVM.Hranas = hranaVms;
            searchVM.TotalCount = filteredCount;
            searchVM.LastPage = (int)Math.Ceiling((double)filteredCount / searchVM.PageSize);

            ViewBag.Categories = GetCategoryListItems();
            return View(searchVM);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Create()
    {
        var vm = new HranaVM
        {
            AlergeniDdl = GetAlergenListItems()
        };
        ViewBag.CategoryDdlItems = GetCategoryListItems();
        return View(vm);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(HranaVM foodVm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryDdlItems = GetCategoryListItems();
                foodVm.AlergeniDdl = GetAlergenListItems();
                return View(foodVm);
            }

            var newFood = _mapper.Map<Hrana>(foodVm);
            _context.Hranas.Add(newFood);
            _context.SaveChanges();

            foreach (var alergenId in foodVm.OdabraniAlergeni)
            {
                _context.HranaAlergens.Add(new HranaAlergen
                {
                    HranaId = newFood.Idhrana,
                    AlergenId = alergenId
                });
            }
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Hrana '{newFood.Naslov}' je uspješno kreirana.";
            return RedirectToAction("Search");
        }
        catch
        {
            ModelState.AddModelError("", "Greška pri kreiranju hrane.");
            ViewBag.CategoryDdlItems = GetCategoryListItems();
            foodVm.AlergeniDdl = GetAlergenListItems();
            return View(foodVm);
        }
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Edit(int id)
    {
        try
        {
            var foodItem = _context.Hranas
                .Include(x => x.KategorijaHrane)
                .Include(h => h.HranaAlergens)
                .FirstOrDefault(x => x.Idhrana == id);

            if (foodItem == null)
                return NotFound();

            var foodVm = _mapper.Map<HranaVM>(foodItem);
            foodVm.OdabraniAlergeni = foodItem.HranaAlergens
                .Where(ha => ha.AlergenId.HasValue)
                .Select(ha => ha.AlergenId.Value)
                .ToList();
            foodVm.AlergeniDdl = GetAlergenListItems();
            ViewBag.CategoryDdlItems = GetCategoryListItems();

            return View(foodVm);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, HranaVM vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryDdlItems = GetCategoryListItems();
                vm.AlergeniDdl = GetAlergenListItems();
                return View(vm);
            }

            var hrana = _context.Hranas.FirstOrDefault(h => h.Idhrana == id);
            if (hrana == null)
                return NotFound();

            _mapper.Map(vm, hrana);
            _context.SaveChanges();

            var stari = _context.HranaAlergens.Where(x => x.HranaId == id);
            _context.HranaAlergens.RemoveRange(stari);

            foreach (var alergenId in vm.OdabraniAlergeni)
            {
                _context.HranaAlergens.Add(new HranaAlergen
                {
                    HranaId = id,
                    AlergenId = alergenId
                });
            }
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Hrana je uspješno ažurirana.";
            return RedirectToAction("Search");
        }
        catch
        {
            ModelState.AddModelError("", "Greška pri ažuriranju hrane.");
            ViewBag.CategoryDdlItems = GetCategoryListItems();
            vm.AlergeniDdl = GetAlergenListItems();
            return View(vm);
        }
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        try
        {
            var foodItem = _context.Hranas
                .Include(x => x.KategorijaHrane)
                .FirstOrDefault(x => x.Idhrana == id);

            if (foodItem == null)
                return NotFound();

            var foodVm = _mapper.Map<HranaVM>(foodItem);
            return View(foodVm);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, HranaVM hranaVM)
    {
        try
        {
            var dbFood = _context.Hranas.FirstOrDefault(x => x.Idhrana == id);
            if (dbFood == null)
                return NotFound();

            _context.Hranas.Remove(dbFood);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Hrana je uspješno obrisana.";
            return RedirectToAction("Search");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Greška pri brisanju hrane.");
            return View(hranaVM);
        }
    }
}
