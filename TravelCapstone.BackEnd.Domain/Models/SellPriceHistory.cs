﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class SellPriceHistory
{
    [Key] public Guid Id { get; set; }
    public int MOQ { get; set; }
    public double PricePerAdult { get; set; }
    public double PricePerChild { get; set; }
    public DateTime Date { get; set; }
    public Guid ServiceId { get; set; }

    [ForeignKey(nameof(ServiceId))] public Service? Service { get; set; }
}