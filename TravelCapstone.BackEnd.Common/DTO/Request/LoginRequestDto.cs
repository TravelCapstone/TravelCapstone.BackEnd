﻿namespace TravelCapstone.BackEnd.Common.DTO.Request;

public class LoginRequestDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}