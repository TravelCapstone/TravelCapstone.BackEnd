﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public double Total { get; set; }
        public string Content { get; set; } = null!;
        public OrderStatus OrderStatus { get; set; }
        public int NumOfAdult { get; set; }
        public int NumOfChildren { get; set;}
    }
}
