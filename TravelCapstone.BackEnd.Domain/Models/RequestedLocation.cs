using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class RequestedLocation
{
    [Key]
    public Guid Id { get; set; }
    public Guid ProvinceId { get; set; }
    [ForeignKey(nameof(ProvinceId))]
    public Province? Province { get; set; }
}