using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models.BaseModel;

namespace TravelCapstone.BackEnd.Domain.Models;

public class PrivateJoinTourRequest: BaseEntity
{
    [Key] public Guid Id { get; set; }

    public Guid TourId { get; set; }

    [ForeignKey(nameof(TourId))] public Tour Tour { get; set; } = null!;

    public Guid TravelCompanionId { get; set; }

    [ForeignKey(nameof(TravelCompanionId))]
    public Customer? TravelCompanion { get; set; }

    public int NumOfAdult { get; set; }
    public int NumOfChildren { get; set; }
    public Enum.JoinTourStatus JoinTourStatusId { get; set; }
    [ForeignKey(nameof(JoinTourStatusId))]
    public Models.EnumModels.JoinTourStatus? JoinTourStatus { get; set; }
}