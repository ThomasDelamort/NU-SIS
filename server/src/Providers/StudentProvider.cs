using Microsoft.EntityFrameworkCore;
using server.src.Data;
using server.src.Models;

namespace server.src.Providers;

public class StudentProvider
{
    private readonly ApplicationDbContext _context;

    public StudentProvider(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET ALL
    public async Task<List<Student>> GetStudents()
    {
        return await _context.Students.ToListAsync();
    }

    // GET BY ID
    public async Task<Student?> GetStudentById(int studentId)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
    }

    public async Task<Student?> GetStudentByRfid(string rfidUid)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.RfidUid == rfidUid);
    }

    public async Task<Student> CreateStudent(Student student)
    {
        _context.Students.Add(student);

        await _context.SaveChangesAsync();

        return student;
    }

    public async Task<Student?> UpdateStudent(
        int studentId,
        Student updatedStudent)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.StudentId == studentId);

        if (student == null)
            return null;

        // Update properties here

        await _context.SaveChangesAsync();

        return student;
    }

    public async Task<bool> DeleteStudent(int studentId)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.StudentId == studentId);

        if (student == null)
            return false;

        _context.Students.Remove(student);

        await _context.SaveChangesAsync();

        return true;
    }
}