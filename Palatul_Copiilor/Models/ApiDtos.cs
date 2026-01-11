namespace Palatul_Copiilor.Models
{
    public record LoginRequest(string Email, string Password);
    public record LoginResponse(string Token, DateTime ExpiresAtUtc);

    public record ReservationDto(
        int Id,
        DateTime ReservedAt,
        int ActivityId,
        string ActivityTitle,
        DateTime StartAt,
        string? DepartmentName,
        string? TeacherName
    );
}
