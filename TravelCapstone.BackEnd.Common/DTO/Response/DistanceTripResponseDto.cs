using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class DistanceTripResponseDto
    {
        public VehicleType VehicleType { get; set; }
        public string DistanceInText { get; set; } = null!;
        public string DurationInText { get; set; } = null!;
        public int NumOfDay { get; set; }   
        public int NumOfNight { get; set; } 
    }
}
