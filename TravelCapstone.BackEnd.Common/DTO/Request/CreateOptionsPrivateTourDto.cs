using Microsoft.EntityFrameworkCore.Diagnostics;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Request;

public class CreateOptionsPrivateTourDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<HumanResourceCost> TourGuideCosts { get; set; } = new List<HumanResourceCost>();
    public List<MaterialCost> MaterialCosts { get; set; } = new List<MaterialCost>();
    public Guid AssurancePriceHistoryOptionId { get; set; }
    public double AssurancePricePerPerson { get; set; }
    public double OrganizationCost { get; set; }
    public double ContingencyFee { get; set; }
    public double EscortFee { get; set; }
    public double OperatingFee { get; set; }
    public Guid PrivateTourRequestId { get; set; }
    public List<ProvinceService> provinceServices { get; set; } = new List<ProvinceService>();
    public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}


public class HumanResourceCost
{
    public int Quantity { get; set; }
    public int NumOfDay { get; set; }
    public Guid? ProvinceId { get; set; }
}

public class MaterialCost
{
    public Guid MaterialPriceHistoryId { get; set; }
    public int Quantity { get; set; }
}



public class ProvinceService
{
    public List<Hotel> Hotels { get; set; } = new List<Hotel>();
    public List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    public List<Entertainment> Entertainments { get; set; } = new List<Entertainment>();   
    public List<EventGala>? EventGalas { get; set; } = new List<EventGala>();
}

public class EventGala
{
    public DateTime Date { get; set; }
    public Guid EventId { get; set; }
    public string CustomEvent { get; set; }
}

public class Hotel
{
    public Guid DistrictId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumOfSingleRoom { get; set; }
    public int NumOfDoubleRoom { get; set; }
    public Rating? HotelOptionRatingOption1 {  get; set; }
    public Rating? HotelOptionRatingOption2 {  get; set; }
    public Rating? HotelOptionRatingOption3 {  get; set; }
}

public class Restaurant
{
    public Guid DistrictId { get; set; }
    public List<MenuQuotation> MenuQuotations { get; set; } = new List<MenuQuotation>();
}

public class MenuQuotation
{
    public DateTime Date { get; set; }
    public OptionClass option {  get; set; }
    public List<Guid> MenuIds { get; set; } = new List<Guid>();
}

public class Entertainment
{
    public Guid DistrictId { get; set; }
    public int? QuantityLocationOption1 { get; set; }
    public int? QuantityLocationOption2 { get; set; }
    public int? QuantityLocationOption3 { get; set; }
}

public class Vehicle
{
    public VehicleType VehicleType { get; set; }
    public Guid? StartPoint { get; set; }
    public Guid? StartPointDistrict { get; set; }
    public Guid? EndPoint { get; set; }
    public Guid? EndPointDistrict { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumOfVehicle { get; set; } = 1;
    public OptionClass? OptionClass1 { get; set; }
    public OptionClass? OptionClass2 { get; set; }
    public OptionClass? OptionClass3 { get; set; }

}





//public class CreateOptionsPrivateTourDto
//{
//    public OptionClass OptionClass { get; set; }
//    public Guid PrivateTourRequestId { get; set; }
//    public List<Hotel> Hotels { get; set; } = new List<Hotel>();
//    public List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
//    public List<Entertainment> Entertainments { get; set; } = new List<Entertainment>();
//    public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

//}


//public class Hotel
//{
//    public Guid DistrictId { get; set; }
//    public int NumOfDay { get; set; }
//    public DateTime StartDate { get; set; }
//    public DateTime EndDate { get; set; }
//    public Rating Rating { get; set; }
//    public int ServingQuantity { get; set; }
//    public int NumOfRoom { get; set; }
//}

//public class Restaurant
//{
//    public Guid DistrictId { get; set; }
//    public DateTime StartDate { get; set; }
//    public DateTime EndDate { get; set; }
//    public Rating Rating { get; set; }
//    public int MealPerDay { get; set; } = 1;
//    public int NumOfDay { get; set; } = 1;
//    public int ServingQuantity { get; set; } = 10;
//    public ServiceAvailability ServiceAvailability { get; set; } = ServiceAvailability.BOTH;
//}
//public class Entertainment
//{
//    public Guid DistrictId { get; set; }
//    public int QuantityLocation { get; set; }
//}

//public class Vehicle
//{
//    public VehicleType VehicleType { get; set; }
//    public Guid? StartPoint { get; set; }
//    public Guid? StartPointDistrict { get; set; }
//    public Guid? EndPoint { get; set; }
//    public Guid? EndPointDistrict { get; set; }
//    public int NumOfRentingDay { get; set; } = 1;
//    public int NumOfVehicle { get; set; } = 1;
//}
