using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models.EnumModels;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class ServiceRating
    {
        [Key]
        public Guid Id { get; set; }
        public Enum.ServiceType ServiceTypeId { get; set; }
        [ForeignKey(nameof(ServiceTypeId))]
        public EnumModels.ServiceType? ServiceType { get; set; } 
        public int Rating { get; set; }
    }
}
