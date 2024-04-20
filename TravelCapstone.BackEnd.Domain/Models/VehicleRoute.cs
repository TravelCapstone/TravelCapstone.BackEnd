using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class VehicleRoute
{
    [Key] public Guid Id { get; set; }

    public Guid VehicleId { get; set; }

    [ForeignKey(nameof(VehicleId))] public Vehicle? Vehicle { get; set; }

    public Guid RouteId { get; set; }

    [ForeignKey(nameof(RouteId))] public Route? Route { get; set; }

    public Guid DriverId { get; set; }

    [ForeignKey(nameof(DriverId))] public Driver? Driver { get; set; }

    public Guid ReferenceTransportPriceId { get; set; }

    [ForeignKey(nameof(ReferenceTransportPriceId))] public ReferenceTransportPrice? ReferenceTransportPrice { get; set; }
}