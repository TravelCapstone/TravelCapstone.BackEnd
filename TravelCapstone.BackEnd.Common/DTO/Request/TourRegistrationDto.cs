using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class TourRegistrationDto
    {
        public Guid? Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; } 
        public bool Gender { get; set; } = true;
        public DateTime Dob { get; set; }
        public bool IsAdult { get; set; }
        public List<TravelCompanion> TravelCompanions { get; set; } = new List<TravelCompanion>();
        public Guid? TourId { get; set; }   
    }

    public class TravelCompanion
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool Gender { get; set; } = true;
        public DateTime Dob { get; set; }
        public bool IsAdult { get; set; }
    }
}
