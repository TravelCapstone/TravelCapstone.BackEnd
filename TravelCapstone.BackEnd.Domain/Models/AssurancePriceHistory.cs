using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class AssurancePriceHistory
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public Guid AssuranceId { get; set; }
        [ForeignKey(nameof(AssuranceId))]
        public Assurance? Assurance { get; set; }
    }
}
