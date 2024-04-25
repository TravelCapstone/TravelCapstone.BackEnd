using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Province
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public double? lat { get; set; }
    public double? lng { get; set; }
}