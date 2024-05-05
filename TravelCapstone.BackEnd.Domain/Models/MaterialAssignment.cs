using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class MaterialAssignment
{
    [Key] 
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public Guid TourId { get; set; }
    [ForeignKey(nameof(TourId))] public Tour? Tour { get; set; }
    public Guid MaterialId { get; set; }
    [ForeignKey(nameof(MaterialId))]
    public Material? Material { get; set; }
}