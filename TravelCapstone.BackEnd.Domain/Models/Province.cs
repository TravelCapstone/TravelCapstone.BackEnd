using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class Province
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }= null!;
    }
}
