using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Driver
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public DateTime DayOfBirth { get; set; }
}