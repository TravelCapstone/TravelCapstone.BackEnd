using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Commune
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public Guid DistrictId { get; set; }

    [ForeignKey(nameof(DistrictId))] public District? District { get; set; }
}