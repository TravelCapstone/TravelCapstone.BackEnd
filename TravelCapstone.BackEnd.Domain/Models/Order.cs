using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Order
{
    [Key] public Guid Id { get; set; }

    public double Total { get; set; }
    public string Content { get; set; } = null!;
    public OrderStatus OrderStatus { get; set; }
    public int NumOfAdult { get; set; }
    public int NumOfChildren { get; set; }
    public Guid TravelCompanionId { get; set; }

    [ForeignKey(nameof(TravelCompanionId))]
    public TravelCompanion? TravelCompanion { get; set; }
    
    public Guid TourId { get; set; }
    [ForeignKey(nameof(TourId))]
    public Tour? Tour { get; set; }
}