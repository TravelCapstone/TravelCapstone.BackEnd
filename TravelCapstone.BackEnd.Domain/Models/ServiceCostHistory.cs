using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class ServiceCostHistory
{
    [Key] public Guid Id { get; set; }

    public double Price { get; set; }
    public int MOQ { get; set; }
    public DateTime Date { get; set; }
    public Guid ServiceId { get; set; }

    [ForeignKey(nameof(ServiceId))] public Service? Service { get; set; }
}