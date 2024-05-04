using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class CreatePlanDetailDto
    {
        public List<PlanLocation> Locations {  get; set; } = new List<Location>();
    }

    public class Tourguide
    {
        public string TourguideId { get; set; } = null!;
        public Guid ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
    }

    public class PlanLocation
    {
        public Guid FacilityId { get; set; }
        public Guid SellPriceHistoryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumOfServiceUse { get; set; }
    }

    public class EatingPlanLocation : PlanLocation
    {
        public int MealPerDay { get; set; }
        public List<DateTime> eatingTimes { get; set; } = new List<DateTime> { };
    }

    public class PlanVehicle
    {
        public VehicleType VehicleType { get; set; }
        public Guid? VehicleId { get; set; }
        public Guid? StartPoint { get; set; }
        public Guid? EndPoint { get; set; }
        public Guid? StartPointDistrict { get; set; }
        public Guid? EndPointDistrict { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? DriverId { get; set; }
        public Guid? SellPriceHistoryId { get; set; }
        public Guid? ReferencePriceId { get; set; }
        public int NumOfVehicle { get; set; } = 0;
    }
}
