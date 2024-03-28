using Microsoft.AspNetCore.Identity;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Account : IdentityUser
{
    public override string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool Gender { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsVerified { get; set; } = false;
    public string? VerifyCode { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}