using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Route
{
    [Key] public Guid Id { get; set; }
    public string? Note { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public Guid DayPlanId { get; set; }

    [ForeignKey(nameof(DayPlanId))] public DayPlan? DayPlan { get; set; }

    public Guid? StartPointId { get; set; }

    [ForeignKey(nameof(StartPointId))] public Facility? StartPoint { get; set; }

    public Guid? EndPointId { get; set; }

    [ForeignKey(nameof(EndPointId))] public Facility? EndPoint { get; set; }
    public Guid? PortStartPointId { get; set; }

    [ForeignKey(nameof(PortStartPointId))] public Port? PortStartPoint { get; set; }

    public Guid? PortEndPointId { get; set; }

    [ForeignKey(nameof(PortEndPointId))] public Port? PortEndPoint { get; set; }

    public Guid? ParentRouteId { get; set; }

    [ForeignKey(nameof(ParentRouteId))] public Route? ParentRoute { get; set; }
}