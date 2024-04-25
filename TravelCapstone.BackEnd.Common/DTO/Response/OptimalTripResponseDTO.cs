using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class OptimalTripResponseDTO
    {
        public List<RouteNode> OptimalTrip { get; set; } = new List<RouteNode>();
        public double TotalDistance { get; set; }
        public double TotalDuration { get; set; }
    }

    public class RouteNode
    {
        public int Index { get; set; }
        public Guid ProvinceId { get; set; }
        public string ProvinceName { get; set; } = null;
        public VehicleType VehicleToNextDestination { get; set; }
        public double Duration { get; set; }
        public double DistanceToNextDestination { get; set; }
    }
}
