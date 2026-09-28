using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.src.Data;
using server.src.Dtos;
using server.src.Models;

namespace server.src.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public StudentsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentResponse>>> GetAll()
    {
        var students = await _db.Students
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .Select(s => new StudentResponse(
                s.StudentId, s.RfidUid, s.FirstName, s.LastName, s.UniversityId,
                s.UniversityEmail, s.ContactNumber, s.ProfilePicture, s.EmergencyContactNumber, s.CreatedAt))
            .ToListAsync();

        return Ok(students);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentResponse>> GetById(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
            return NotFound();

        return Ok(ToResponse(student));
    }

    [HttpGet("rfid/{rfidUid}")]
    public async Task<ActionResult<StudentResponse>> GetByRfid(string rfidUid)
    {
        var student = await _db.Students.SingleOrDefaultAsync(s => s.RfidUid == rfidUid);
        if (student is null)
            return NotFound();

        return Ok(ToResponse(student));
    }

    [HttpPost]
    public async Task<ActionResult<StudentResponse>> Create(StudentCreateRequest request)
    {
        if (await _db.Students.AnyAsync(s => s.RfidUid == request.RfidUid))
            return Conflict($"A student with RFID '{request.RfidUid}' already exists.");

        if (await _db.Students.AnyAsync(s => s.UniversityId == request.UniversityId))
            return Conflict($"A student with university ID '{request.UniversityId}' already exists.");

        var student = new Student
        {
            RfidUid = request.RfidUid,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UniversityId = request.UniversityId,
            UniversityEmail = request.UniversityEmail,
            ContactNumber = request.ContactNumber,
            ProfilePicture = request.ProfilePicture,
            EmergencyContactNumber = request.EmergencyContactNumber,
            CreatedAt = DateTime.UtcNow,
        };

        _db.Students.Add(student);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = student.StudentId }, ToResponse(student));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, StudentUpdateRequest request)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
            return NotFound();

        if (await _db.Students.AnyAsync(s => s.StudentId != id && s.RfidUid == request.RfidUid))
            return Conflict($"A student with RFID '{request.RfidUid}' already exists.");

        if (await _db.Students.AnyAsync(s => s.StudentId != id && s.UniversityId == request.UniversityId))
            return Conflict($"A student with university ID '{request.UniversityId}' already exists.");

        student.RfidUid = request.RfidUid;
        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.UniversityId = request.UniversityId;
        student.UniversityEmail = request.UniversityEmail;
        student.ContactNumber = request.ContactNumber;
        student.ProfilePicture = request.ProfilePicture;
        student.EmergencyContactNumber = request.EmergencyContactNumber;

        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
            return NotFound();

        _db.Students.Remove(student);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private static StudentResponse ToResponse(Student s) => new(
        s.StudentId, s.RfidUid, s.FirstName, s.LastName, s.UniversityId,
        s.UniversityEmail, s.ContactNumber, s.ProfilePicture, s.EmergencyContactNumber, s.CreatedAt);
}
