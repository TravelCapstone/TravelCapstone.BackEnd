using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; } = null!; 
        public string Description { get; set; } = null!;
        public int MOQ { get; set; }
    }
}
