using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class TravelCompanion
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; }=null!;
        public bool Gender { get; set; } = true;
        public DateTime Dob {  get; set; }
        public bool IsAdult { get; set; }
        public double Money { get; set; }
        public string AccountId { get; set; } = null!;
        [ForeignKey (nameof(AccountId))]
        public Account? Account { get; set; }
    }
}
