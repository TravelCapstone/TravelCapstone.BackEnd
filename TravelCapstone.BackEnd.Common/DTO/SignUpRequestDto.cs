﻿namespace TravelCapstone.BackEnd.Common.DTO;

public class SignUpRequestDto
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool Gender { get; set; }
    public string PhoneNumber { get; set; } = null!;
}