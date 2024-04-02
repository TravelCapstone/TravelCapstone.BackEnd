using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class DayPlan
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid TourId { get; set; }

    [ForeignKey(nameof(TourId))] public Tour? Tour { get; set; }
}