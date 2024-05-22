using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Tour
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Enum.VehicleType VehicleTypeId { get; set; }
    [ForeignKey(nameof(VehicleTypeId))]
    public EnumModels.VehicleType? VehicleType { get; set; }
    public double OrganizationCost { get; set; }
    public double ContingencyFee { get; set; }
    public double EscortFee { get; set; }
    public double OperatingFee { get; set; }
    public double TotalPrice { get; set; }
    public double PricePerAdult { get; set; }
    public double PricePerChild { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Enum.TourType TourTypeId { get; set; }
    [ForeignKey(nameof(TourTypeId))]
    public EnumModels.TourType? TourType { get; set; }
    public string? QRCode { get; set; }
    public Enum.TourStatus TourStatusId { get; set; }
    [ForeignKey(nameof(TourStatusId))]
    public EnumModels.TourStatus? TourStatus { get; set; }
    public Guid? BasedOnTourId { get; set; }

    [ForeignKey(nameof(BasedOnTourId))]
    public Tour? BasedTour { get; set; }

}