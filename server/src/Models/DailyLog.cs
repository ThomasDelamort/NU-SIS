using System.ComponentModel.DataAnnotations.Schema;

namespace server.src.Models;

public class DailyLog
{
    public int LogId { get; set; }

    public int StudentId { get; set; }

    public DateTimeOffset TimeIn { get; set; }

    public DateTimeOffset? TimeOut { get; set; }

    [ForeignKey(nameof(StudentId))]
    public Student Student { get; set; } = null!;
}
