using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Tour
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public MainVehicleTour MainVehicle { get; set; }
    public double TotalPrice { get; set; }
    public double PricePerAdult { get; set; }
    public double PricePerChild { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TourType TourType { get; set; }
    public string? QRCode { get; set; }
    public TourStatus TourStatus { get; set; }
    public Guid? BasedOnTourId { get; set; }

    [ForeignKey(nameof(BasedOnTourId))]
    public Tour? BasedTour { get; set; }

    public string? TourGuideId { get; set; }

    [ForeignKey(nameof(TourGuideId))]
    public Account? TourGuide { get; set; }
}