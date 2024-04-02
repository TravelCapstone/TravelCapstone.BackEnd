using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class District
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public Guid ProvinceId { get; set; }

    [ForeignKey(nameof(ProvinceId))] public Province? Province { get; set; }
}