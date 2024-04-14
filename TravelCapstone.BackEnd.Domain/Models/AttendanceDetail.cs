using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class AttendanceDetail
{
    [Key] public Guid Id { get; set; }
    public AttendanceType AttendanceTypeId { get; set; }
    [ForeignKey(nameof(AttendanceTypeId))]
    public Models.EnumModels.AttendanceType? AttendanceType { get; set; }
    public Guid TourTravellerId { get; set; }

    [ForeignKey(nameof(TourTravellerId))] public TourTraveller? TourTraveller { get; set; }

    public Guid AttendanceRouteId { get; set; }

    [ForeignKey(nameof(AttendanceRouteId))]
    public AttendanceRoute? AttendanceRoute { get; set; }
}