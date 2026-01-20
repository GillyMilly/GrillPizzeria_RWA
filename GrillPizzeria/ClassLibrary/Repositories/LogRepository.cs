using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Repositories;

public class LogRepository
{
    private readonly GrillPizzeriaDbContext _context;

    public LogRepository(GrillPizzeriaDbContext context)
    {
        _context = context;
    }

    public void AddLog(Log log)
    {
        _context.Logs.Add(log);
        _context.SaveChanges();
    }

    public async Task AddLogAsync(Log log)
    {
        _context.Logs.Add(log);
        await _context.SaveChangesAsync();
    }

    public List<Log> GetLogs(int last)
    {
        return _context.Logs
            .OrderByDescending(l => l.Timestamp)
            .Take(last)
            .ToList();
    }

    public async Task<List<Log>> GetLogsAsync(int last)
    {
        return await _context.Logs
            .OrderByDescending(l => l.Timestamp)
            .Take(last)
            .ToListAsync();
    }

    public int GetLogCount()
    {
        return _context.Logs.Count();
    }

    public async Task<int> GetLogCountAsync()
    {
        return await _context.Logs.CountAsync();
    }

    public async Task<List<Log>> GetAllLogsAsync()
    {
        return await _context.Logs
            .OrderByDescending(l => l.Timestamp)
            .ToListAsync();
    }
}
