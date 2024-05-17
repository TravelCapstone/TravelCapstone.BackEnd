using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class MaterialPriceHistory
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public Guid MaterialId { get; set; }
        [ForeignKey(nameof(MaterialId))]
        public Material? Material { get; set; }
    }
}
