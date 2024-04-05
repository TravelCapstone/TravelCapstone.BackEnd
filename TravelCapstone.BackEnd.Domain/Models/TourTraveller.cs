using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class TourTraveller
{
    [Key] public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer? Customer { get; set; }

    public Guid TourId { get; set; }

    [ForeignKey(nameof(TourId))] public Tour? Tour { get; set; }
}