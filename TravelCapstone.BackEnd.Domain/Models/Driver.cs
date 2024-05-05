using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Driver
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public DateTime DayOfBirth { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public double FixDriverSalary { get; set; }
    public bool IsActive { get; set; }
}