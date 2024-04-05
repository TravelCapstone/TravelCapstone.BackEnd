using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class PlanServiceCostDetail
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid TourId { get; set; }
    public int Quantity { get; set; }
    [ForeignKey(nameof(TourId))] public Tour Tour { get; set; } = null!;

    public Guid? SellPriceHistoryId { get; set; }

    [ForeignKey(nameof(SellPriceHistoryId))]
    public SellPriceHistory? SellPriceHistory { get; set; }
}