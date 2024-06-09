using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class FamilyDetailRequest
    {
        [Key]
        public Guid Id { get; set; }
        public int NumOfChildren { get; set; }
        public int NumOfAdult { get; set; }
        public int TotalFamily { get; set; }
        public Guid PrivateTourRequestId { get; set; }
        [ForeignKey(nameof(PrivateTourRequestId))]
        public PrivateTourRequest? PrivateTourRequest { get; set; }
    }
}
