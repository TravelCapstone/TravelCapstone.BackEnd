using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class OrderDetail
{
    [Key] public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    [ForeignKey(nameof(OrderId))] public Order Order { get; set; } = null!;

    public Guid TravelCompanionId { get; set; }

    [ForeignKey(nameof(TravelCompanionId))]
    public TravelCompanion? TravelCompanion { get; set; }
}