using AutoMapper;
using ClassLibrary.Models;
using ClassLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize(Roles = "Admin")]
public class LogController : Controller
{
    private readonly LogRepository _logRepository;
    private readonly IMapper _mapper;

    public LogController(LogRepository logRepository, IMapper mapper)
    {
        _logRepository = logRepository;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 25)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 25;

            var allLogs = await _logRepository.GetAllLogsAsync();
            var totalCount = allLogs.Count;
            var lastPage = (int)Math.Ceiling((double)totalCount / pageSize);

            var logs = allLogs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var logVms = _mapper.Map<List<LogVM>>(logs);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.LastPage = lastPage;

            return View(logVms);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }
}
