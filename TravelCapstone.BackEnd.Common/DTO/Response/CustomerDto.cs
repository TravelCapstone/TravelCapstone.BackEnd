using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class CustomerDto
    {
        public Guid? Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Gender { get; set; } = true;
        public DateTime Dob { get; set; }
        public bool IsAdult { get; set; }
        public double Money { get; set; }
        public string? AccountId { get; set; }
        public bool IsVerfiedNPhoneNumber { get; set; } = false;
        public bool IsVerifiedEmail { get; set; } = false;
        public AccountResponse? Account { get; set; }
    }
}
