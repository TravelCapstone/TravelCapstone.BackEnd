namespace TravelCapstone.BackEnd.Common.DTO;

public class SignUpRequestDto
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public bool Gender { get; set; }
    public string PhoneNumber { get; set; }
}