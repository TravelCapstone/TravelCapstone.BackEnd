﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Customer
{
    [Key] public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool Gender { get; set; } = true;
    public DateTime Dob { get; set; }
    public bool IsAdult { get; set; }
    public double Money { get; set; }
    public string? AccountId { get; set; }
    public bool IsVerfiedPhoneNumber { get; set; } = false;
    public bool IsVerifiedEmail { get; set; } = false;
    public string? VerficationCodePhoneNumber { get; set; }
    public string? VerficationCodeEmail { get; set; }

    [ForeignKey(nameof(AccountId))] public Account? Account { get; set; }
}