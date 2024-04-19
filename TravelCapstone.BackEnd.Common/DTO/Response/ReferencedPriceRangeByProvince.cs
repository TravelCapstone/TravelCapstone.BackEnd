using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class ReferencedPriceRangeByProvince
    {
        public PriceReference HotelPrice = null!;
        public PriceReference RestaurantPrice = null!;
        public PriceReference EntertainmentPrice = null!;
        public PriceReference VehicleSupplyPrice = null!;
    }

    public class PriceReference
    {
        public ServiceType ServiceType;
        public List<DetailedPriceReference> DetailedPriceReferences;
        public PriceReference(ServiceType serviceType)
        {
            this.ServiceType = serviceType;
            DetailedPriceReferences = new List<DetailedPriceReference>();
        }
    }
    public class DetailedPriceReference
    {
        public ServiceRating ServiceRating = null!;
        public ServiceAvailability ServiceAvailability;
        public Unit Unit;
        public int ServingQuantity;
        public double MaxSurChange;
        public double MinSurChange;
        public double MaxPrice;
        public double MinPrice;
    }
}
