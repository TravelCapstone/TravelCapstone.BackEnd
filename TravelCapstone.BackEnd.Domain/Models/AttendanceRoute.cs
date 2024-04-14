using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class AttendanceRoute
{
    [Key] public Guid Id { get; set; }

    public AttendanceRouteType AttendanceRouteTypeId { get; set; }
    [ForeignKey(nameof(AttendanceRouteTypeId))]
    public Models.EnumModels.AttendanceRouteType? AttendanceRouteType { get; set; }
    public Guid RouteId { get; set; }

    [ForeignKey(nameof(RouteId))] public Route? Route { get; set; }
}