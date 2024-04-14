using System.ComponentModel.DataAnnotations;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class AttendanceRouteType
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.AttendanceRouteType Id { get; set; }
    public string Name { get; set; } = null!;
}