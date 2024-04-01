using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class PrivateJoinTourRequest
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TourId { get; set; }
        [ForeignKey(nameof(TourId))]
        public Tour Tour { get; set; } = null!;
        public Guid TravelCompanionId { get; set; }
        [ForeignKey(nameof(TravelCompanionId))]
        public TravelCompanion? TravelCompanion { get; set; } 
        public int NumOfAdult {  get; set; }
        public int NumOfChildren { get; set; }
        public JoinTourStatus Status { get; set; }

    }
}
