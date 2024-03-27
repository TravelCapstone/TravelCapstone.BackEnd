namespace TravelCapstone.BackEnd.Common.DTO;

public class ForgotPasswordDto
{
    public string Email { get; set; }
    public string RecoveryCode { get; set; }
    public string NewPassword { get; set; }
}