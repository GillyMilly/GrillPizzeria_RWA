using ClassLibrary.Models;
using ClassLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTO;

namespace WebAPI.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/logs")]
[ApiController]
public class LogController : ControllerBase
{
    private readonly LogRepository _logRepository;

    public LogController(LogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    [HttpGet("get/{last}")]
    public ActionResult<IEnumerable<LogDto>> GetLogs(int last = 10)
    {
        var logs = _logRepository.GetLogs(last);
        return logs.Count > 0 ? Ok(logs) : NotFound("Nema pronađenih logova.");
    }

    [HttpGet("count")]
    public ActionResult<int> GetLogCount()
    {
        return Ok(new { Count = _logRepository.GetLogCount() });
    }

    [HttpPost]
    public ActionResult AddLog([FromBody] LogDto logDto)
    {
        if (logDto == null)
            return BadRequest("Log podaci su obavezni.");

        var log = new Log
        {
            Level = logDto.Level,
            Message = logDto.Message,
            Timestamp = logDto.Timestamp ?? DateTime.UtcNow
        };

        _logRepository.AddLog(log);
        return CreatedAtAction(nameof(GetLogs), new { last = 1 }, logDto);
    }
}
