using System.ComponentModel.DataAnnotations;

namespace server.src.Dtos;

public record DailyLogResponse(
    int LogId,
    int StudentId,
    string StudentName,
    DateTimeOffset TimeIn,
    DateTimeOffset? TimeOut);

public record RfidTapRequest(
    [Required, MaxLength(15)] string RfidUid);

public record TapResult(string Action, DailyLogResponse Log);
