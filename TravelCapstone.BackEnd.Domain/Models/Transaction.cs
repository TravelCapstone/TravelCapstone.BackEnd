﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Transaction
{
    [Key] public Guid Id { get; set; }

    public double Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType TransactionType { get; set; }
    public Guid TravelCompanionId { get; set; }

    [ForeignKey(nameof(TravelCompanionId))]
    public TravelCompanion? TravelCompanion { get; set; }
}