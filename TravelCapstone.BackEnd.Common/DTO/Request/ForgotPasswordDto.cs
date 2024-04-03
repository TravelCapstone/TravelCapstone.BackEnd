namespace TravelCapstone.BackEnd.Common.DTO.Request;

public class ForgotPasswordDto
{
    public string Email { get; set; } = null!;
    public string? RecoveryCode { get; set; }
    public string? NewPassword { get; set; }
}