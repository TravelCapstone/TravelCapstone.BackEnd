using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class AttendanceType
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.AttendanceType Id { get; set; }
    public string Name { get; set; } = null!;
}