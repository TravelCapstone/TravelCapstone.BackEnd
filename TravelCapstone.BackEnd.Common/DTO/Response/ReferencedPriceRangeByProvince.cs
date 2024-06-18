using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class ReferencedPriceRangeByProvince
    {
        public PriceReference HotelPrice { get; set; } = null!;
        public PriceReference RestaurantPrice { get; set; } = null!;
        public PriceReference EntertainmentPrice { get; set; } = null!;
        //public PriceReference VehicleSupplyPrice { get; set; } = null!;
    }

    public class PriceReference
    {
        public ServiceType ServiceType;
        public List<DetailedPriceReference> DetailedPriceReferences {  get; set; } = new List<DetailedPriceReference>();
        public PriceReference(ServiceType serviceType)
        {
            this.ServiceType = serviceType;
        }
    }
    public class DetailedPriceReference
    {
        public Rating RatingId { get; set; }
        public Guid FacilityRatingId { get; set; }
		public ServiceType ServiceTypeId { get; set; }
        public ServiceAvailability ServiceAvailability { get; set; }
        public Unit Unit { get; set; }
        public int ServingQuantity { get; set; }
        public double MaxSurChange { get; set; } = 0;
        public double MinSurChange { get; set; } = double.MaxValue;
        public double MaxPrice { get; set; } = 0;
        public double MinPrice { get; set; } = double.MaxValue;
    }

    public class DetailedServicePriceReference
    {
        public SellPriceHistory SellPriceHistory { get; set; } = null!;
        public FacilityService FacilityServices { get; set; } = null!;
        public double PriceOfPerson { get; set; }
    }
    public class DetailedServiceMealPriceReference
    {
        public SellPriceHistory SellPriceHistory { get; set; } = null!;
        public FacilityService FacilityServices { get; set; } = null!;
        public List<MenuDish> MenuDishes { get; set; } = null!;
        public double PriceOfPerson { get; set; }
    }
}
