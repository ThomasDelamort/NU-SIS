using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.src.Data;
using server.src.Dtos;
using server.src.Models;

namespace server.src.Controllers;

[ApiController]
[Route("api/daily-logs")]
public class DailyLogsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public DailyLogsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost("tap")]
    public async Task<ActionResult<TapResult>> Tap(RfidTapRequest request)
    {
        var student = await _db.Students.SingleOrDefaultAsync(s => s.RfidUid == request.RfidUid);
        if (student is null)
            return NotFound($"No student found with RFID '{request.RfidUid}'.");

        var openLog = await _db.DailyLogs
            .Where(l => l.StudentId == student.StudentId && l.TimeOut == null)
            .OrderByDescending(l => l.TimeIn)
            .FirstOrDefaultAsync();

        string action;
        DailyLog log;

        if (openLog is null)
        {
            log = new DailyLog
            {
                StudentId = student.StudentId,
                TimeIn = DateTimeOffset.UtcNow,
            };
            _db.DailyLogs.Add(log);
            action = "time-in";
        }
        else
        {
            openLog.TimeOut = DateTimeOffset.UtcNow;
            log = openLog;
            action = "time-out";
        }

        await _db.SaveChangesAsync();

        var response = new DailyLogResponse(
            log.LogId, student.StudentId, $"{student.FirstName} {student.LastName}", log.TimeIn, log.TimeOut);

        return Ok(new TapResult(action, response));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DailyLogResponse>>> GetAll()
    {
        var logs = await _db.DailyLogs
            .OrderByDescending(l => l.TimeIn)
            .Select(l => new DailyLogResponse(
                l.LogId, l.StudentId, l.Student.FirstName + " " + l.Student.LastName, l.TimeIn, l.TimeOut))
            .ToListAsync();

        return Ok(logs);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DailyLogResponse>> GetById(int id)
    {
        var log = await _db.DailyLogs
            .Where(l => l.LogId == id)
            .Select(l => new DailyLogResponse(
                l.LogId, l.StudentId, l.Student.FirstName + " " + l.Student.LastName, l.TimeIn, l.TimeOut))
            .SingleOrDefaultAsync();

        if (log is null)
            return NotFound();

        return Ok(log);
    }
}
