using AutoMapper;
using ClassLibrary.Models;
using ClassLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTO;
using WebAPI.Security;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HranaController : ControllerBase
{
    private readonly HranaRepository _hranaRepository;
    private readonly IMapper _mapper;
    private readonly LogRepository _logRepository;

    public HranaController(HranaRepository hranaRepository, IMapper mapper, LogRepository logRepository)
    {
        _hranaRepository = hranaRepository;
        _mapper = mapper;
        _logRepository = logRepository;
    }

    private void LogAction(string level, string message)
    {
        var log = new Log
        {
            Timestamp = DateTime.UtcNow,
            Level = level,
            Message = message
        };
        _logRepository.AddLog(log);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllHrana()
    {
        var hranaList = await _hranaRepository.GetAllHranaAsync();
        var hranaDtos = _mapper.Map<IEnumerable<HranaDto>>(hranaList);
        return hranaDtos.Any() ? Ok(hranaDtos) : NotFound("Nema pronađenih stavki.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHranaById(int id)
    {
        var hrana = await _hranaRepository.GetHranaByIdAsync(id);
        if (hrana == null)
            return NotFound($"Hrana s ID={id} nije pronađena.");

        var hranaDto = _mapper.Map<HranaDto>(hrana);
        return Ok(hranaDto);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchHrana([FromQuery] string? name, [FromQuery] int page = 1, [FromQuery] int count = 5)
    {
        var result = await _hranaRepository.SearchWithPagingAsync(name, page, count);
        var hranaDtos = _mapper.Map<IEnumerable<HranaDto>>(result.Items);

        return Ok(new
        {
            total = result.TotalCount,
            page,
            count,
            items = hranaDtos
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateHrana([FromBody] HranaDto hranaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var hrana = _mapper.Map<Hrana>(hranaDto);
        await _hranaRepository.AddHranaAsync(hrana);

        LogAction("CREATE", $"Hrana s ID={hrana.Idhrana} je stvorena.");
        return CreatedAtAction(nameof(GetHranaById), new { id = hrana.Idhrana }, _mapper.Map<HranaDto>(hrana));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHrana(int id, [FromBody] HranaDto hranaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var hrana = await _hranaRepository.GetHranaByIdAsync(id);
        if (hrana == null)
            return NotFound($"Hrana s ID={id} nije pronađena.");

        _mapper.Map(hranaDto, hrana);
        await _hranaRepository.UpdateHranaAsync(hrana);

        LogAction("UPDATE", $"Hrana s ID={id} je ažurirana.");
        return Ok(_mapper.Map<HranaDto>(hrana));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHrana(int id)
    {
        var hrana = await _hranaRepository.GetHranaByIdAsync(id);
        if (hrana == null)
            return NotFound($"Hrana s ID={id} nije pronađena.");

        await _hranaRepository.DeleteHranaAsync(id);
        LogAction("DELETE", $"Hrana s ID={id} je obrisana.");
        return Ok(new { message = "Hrana je uspješno obrisana.", id });
    }
}
