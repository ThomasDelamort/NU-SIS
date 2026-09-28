using System.ComponentModel.DataAnnotations;

namespace server.src.Dtos;

public record StudentResponse(
    int StudentId,
    string RfidUid,
    string FirstName,
    string LastName,
    string UniversityId,
    string UniversityEmail,
    string? ContactNumber,
    string? ProfilePicture,
    string? EmergencyContactNumber,
    DateTime CreatedAt);

public record StudentCreateRequest(
    [Required, MaxLength(15)] string RfidUid,
    [Required, MaxLength(50)] string FirstName,
    [Required, MaxLength(50)] string LastName,
    [Required, MaxLength(20)] string UniversityId,
    [Required, MaxLength(255)] string UniversityEmail,
    [MaxLength(20)] string? ContactNumber,
    [MaxLength(255)] string? ProfilePicture,
    [MaxLength(20)] string? EmergencyContactNumber);

public record StudentUpdateRequest(
    [Required, MaxLength(15)] string RfidUid,
    [Required, MaxLength(50)] string FirstName,
    [Required, MaxLength(50)] string LastName,
    [Required, MaxLength(20)] string UniversityId,
    [Required, MaxLength(255)] string UniversityEmail,
    [MaxLength(20)] string? ContactNumber,
    [MaxLength(255)] string? ProfilePicture,
    [MaxLength(20)] string? EmergencyContactNumber);
