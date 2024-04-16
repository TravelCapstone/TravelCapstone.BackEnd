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
    public class Port
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public PortType PortType { get; set; }
        public Guid CommuneId { get; set; }
        [ForeignKey(nameof(CommuneId))]
        public Commune? Commune { get; set; }
    }
}
