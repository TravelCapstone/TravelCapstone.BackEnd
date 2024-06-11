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
    public int NumOfFamily { get; set; }
    public int NumOfSingleMale { get; set; }
    public int NumOfSingleFemale { get; set; }
    public Guid? GeneratedTourId { get; set; }
    [ForeignKey(nameof(GeneratedTourId))] public Tour? GeneratedTour{ get; set; }
    public Guid TourId { get; set; }
    [ForeignKey(nameof(TourId))] public Tour? Tour { get; set; }

    public Enum.PrivateTourStatus PrivateTourStatusId { get; set; }
    [ForeignKey(nameof(PrivateTourStatusId))]
    public Models.EnumModels.PrivateTourStatus? PrivateTourStatus { get; set; }
    public bool IsEnterprise { get; set; }
    public string? RecommendedTourUrl { get; set; }
    public string? Note { get; set; }
    public string? StartLocation { get; set; }
    public Guid MinimumHotelRatingId { get; set; }
    [ForeignKey(nameof(MinimumHotelRatingId))]
    public FacilityRating HotelFacilityRating { get; set; }
    public string? SuggestedTourguideName { get; set; }
    public Guid MinimumRestaurantRatingId { get; set; }
    [ForeignKey(nameof(MinimumRestaurantRatingId))]
    public FacilityRating RestaurantFacilityRating { get; set; }    
    public double WishPrice {  get; set; }
    public Enum.DietaryPreference DietaryPreferenceId { get; set; }
    [ForeignKey(nameof(DietaryPreferenceId))]
    public Models.EnumModels.DietaryPreference? DietaryPreference { get; set; }

    public Guid? StartLocationCommuneId { get; set; }
    [ForeignKey(nameof(StartLocationCommuneId))]   
    public Commune? Commune { get; set; }
    public Guid MainDestinationId { get; set; }
    [ForeignKey(nameof(MainDestinationId))]
    public Province? Province { get; set; }
}