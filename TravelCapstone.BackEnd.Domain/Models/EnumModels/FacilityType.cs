﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels
{
    public class FacilityType
    {
        [Key]
        public Enum.FacilityType Id { get; set; }
        public string Name { get; set; } = null!;
    }
}