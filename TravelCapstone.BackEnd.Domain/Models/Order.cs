using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models.EnumModels;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Order
{
    [Key] public Guid Id { get; set; }

    public double Total { get; set; }
    public string Content { get; set; } = null!;
    public Enum.OrderStatus OrderStatusId { get; set; }
    [ForeignKey(nameof(OrderStatusId))]
    public Models.EnumModels.OrderStatus? OrderStatus { get; set; }
    public int NumOfAdult { get; set; }
    public int NumOfChildren { get; set; }
    public Guid CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer? Customer { get; set; }

    public Guid? PrivateTourRequestId { get; set; }

    [ForeignKey(nameof(PrivateTourRequestId))]
    public PrivateTourRequest? PrivateTourRequest { get; set; }

    public Guid? TourId { get; set; }

    [ForeignKey(nameof(TourId))]
    public Tour? Tour { get; set; }
}