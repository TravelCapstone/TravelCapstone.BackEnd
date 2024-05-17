using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class PlanServiceCostDetail
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid TourId { get; set; }
    [ForeignKey(nameof(TourId))] public Tour Tour { get; set; } = null!;
    public Guid? SellPriceHistoryId { get; set; }

    [ForeignKey(nameof(SellPriceHistoryId))]
    public SellPriceHistory? SellPriceHistory { get; set; }

    public Guid? ReferenceTransportPriceId { get; set; }

    [ForeignKey(nameof(ReferenceTransportPriceId))]
    public ReferenceTransportPrice? ReferenceTransportPrice { get; set; }
    public Guid? MaterialPriceHistoryId { get; set; }

    [ForeignKey(nameof(MaterialPriceHistoryId))]
    public MaterialPriceHistory? MaterialPriceHistory { get; set; }
}