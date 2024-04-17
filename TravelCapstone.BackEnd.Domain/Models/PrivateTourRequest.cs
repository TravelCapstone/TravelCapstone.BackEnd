using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models.BaseModel;

namespace TravelCapstone.BackEnd.Domain.Models;

public class PrivateTourRequest: BaseEntity
{
    [Key] public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; } 
    public int NumOfDay { get; set; }   
    public int NumOfNight { get; set; }
    public int NumOfAdult { get; set; }
    public int NumOfChildren { get; set; }
    public Guid TourId { get; set; }
    [ForeignKey(nameof(TourId))] public Tour Tour { get; set; } = null!;
    public Enum.VehicleType MainVehicleId { get; set; }
    [ForeignKey(nameof(MainVehicleId))]
    public Models.EnumModels.VehicleType? VehicleType { get; set; }
    public Enum.PrivateTourStatus PrivateTourStatusId { get; set; }
    [ForeignKey(nameof(PrivateTourStatusId))]
    public Models.EnumModels.PrivateTourStatus? PrivateTourStatus { get; set; }
    public bool IsEnterprise { get; set; }
    public string? RecommnendedTourUrl { get; set; }
    public string? Note { get; set; }

    public Guid MainDestinationId { get; set; }
    [ForeignKey(nameof(MainDestinationId))]
    public Province? Province { get; set; }
    public string? AccountId { get; set; }

    [ForeignKey(nameof(AccountId))] public Account? Account { get; set; }
}