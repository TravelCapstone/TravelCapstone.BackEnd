using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class TourTraveller
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TravelCompanionId { get; set; }
        [ForeignKey(nameof(TravelCompanionId))]
        public TravelCompanion? TravelCompanion { get; set; }
        public Guid TourId { get; set; }
        [ForeignKey(nameof(TourId))]
        public Tour? Tour { get; set; } 
    }
}
