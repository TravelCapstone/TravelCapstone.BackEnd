using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class TourRegistration
{
    [Key] public Guid Id { get; set; }

    public Guid TourId { get; set; }

    [ForeignKey(nameof(TourId))] public Tour? Tour { get; set; }

    public Guid PresenterId { get; set; }

    [ForeignKey(nameof(PresenterId))] public TravelCompanion? Presenter { get; set; }

    public Guid? FollowerId { get; set; }

    [ForeignKey(nameof(FollowerId))] public TravelCompanion? Follower { get; set; }
}