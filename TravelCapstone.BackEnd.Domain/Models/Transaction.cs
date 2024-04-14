using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Transaction
{
    [Key] public Guid Id { get; set; }

    public double Amount { get; set; }
    public DateTime Date { get; set; }
    public Enum.TransactionType TransactionTypeId { get; set; }
    [ForeignKey(nameof(TransactionTypeId))]
    public EnumModels.TransactionType? TransactionType { get; set; }
    public Guid TravelCompanionId { get; set; }

    [ForeignKey(nameof(TravelCompanionId))]
    public Customer? TravelCompanion { get; set; }
}