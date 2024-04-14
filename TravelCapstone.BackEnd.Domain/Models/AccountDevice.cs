using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class AccountDevice
    {
        [Key]
        public Guid Id { get; set; }
        public string? AccountId { get; set; }
        [ForeignKey(nameof(AccountId))] 
        public Account? Account { get; set; }
        public string DeviceId { get; set; } = null!;
    }
}
