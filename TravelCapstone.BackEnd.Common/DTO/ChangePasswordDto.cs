﻿namespace TravelCapstone.BackEnd.Common.DTO;

public class ChangePasswordDto
{
    public string Email { get; set; } = null!;
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}