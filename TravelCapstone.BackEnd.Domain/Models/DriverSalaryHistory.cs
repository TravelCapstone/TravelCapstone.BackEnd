using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class DriverSalaryHistory
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double Salary { get; set; }
        public Guid DriverId { get; set; }
        [ForeignKey(nameof(DriverId))]
        public Driver? Driver { get; set; }
    }
}
