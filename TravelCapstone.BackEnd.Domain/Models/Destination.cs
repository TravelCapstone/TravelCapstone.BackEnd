using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Destination
{
    [Key] public Guid Id { get; set; }

    public string? Name { get; set; }
    public Guid CommunceId { get; set; }
    public Commune? Communce { get; set; }
}