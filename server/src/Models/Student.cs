using System.ComponentModel.DataAnnotations;

namespace server.src.Models;

public class Student
{
    public int StudentId { get; set; }

    [Required]
    [MaxLength(15)]
    public string RfidUid { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string UniversityId { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string UniversityEmail { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? ContactNumber { get; set; }

    [MaxLength(255)]
    public string? ProfilePicture { get; set; }

    [MaxLength(20)]
    public string? EmergencyContactNumber { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<DailyLog> DailyLogs { get; set; } = new List<DailyLog>();
}
