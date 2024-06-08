using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;
using TravelCapstone.BackEnd.Domain.Models.EnumModels;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class CreatePlanDetailDto
    {
        public Guid privateTourRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<PlanLocation> Locations { get; set; } = new List<PlanLocation>();
        public List<PlanVehicle> Vehicles { get; set; } = new List<PlanVehicle>();
        public List<Tourguide> Tourguides { get; set; } = new List<Tourguide>();
        public AddMaterialRequest Material { get; set; }

        public List<DayPlanRoute> DetailPlanRoutes { get; set; } = new List<DayPlanRoute>();
    }

    public class DayPlanRoute
    {
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public List<DetailDayPlanRoute> DetailDayPlanRoutes { get; set; } = new List<DetailDayPlanRoute>();
    }

    public class DetailDayPlanRoute
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note {  get; set; }
        public Guid? StartId { get; set; }
        public Guid? EndId { get; set; }
    }

    public class Tourguide
    {
        public string TourguideId { get; set; } = null!;
        public Guid ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }   
    }

    public class PlanLocation
    {
        public Guid SellPriceHistoryId { get; set; }
        //public Domain.Enum.ServiceType ServiceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int NumOfServiceUse { get; set; }
    }

    public class PlanVehicle
    {
        public Domain.Enum.VehicleType VehicleType { get; set; }
        public Guid? VehicleId { get; set; }
        public Guid? StartPoint { get; set; }
        public Guid? EndPoint { get; set; }
        public Guid? PortStartPoint { get; set; }
        public Guid? PortEndPoint { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? DriverId { get; set; }
        public Guid? SellPriceHistoryId { get; set; }
        public Guid? ReferencePriceId { get; set; }
        public int NumOfVehicle { get; set; } = 0;
    }
}
