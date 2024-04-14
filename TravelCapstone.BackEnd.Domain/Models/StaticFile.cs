using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class StaticFile
    {
        [Key]
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public Guid DestinationId { get; set; }
        [ForeignKey(nameof(DestinationId))]
        public Destination? Destination { get; set; } 
    }
}
