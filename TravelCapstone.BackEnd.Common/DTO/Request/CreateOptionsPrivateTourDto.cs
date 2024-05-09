using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Request;

public class CreateOptionsPrivateTourDto
{
    public OptionClass OptionClass { get; set; }
    public Guid PrivateTourRequestId { get; set; }
    public List<ProvinceService> provinceServices { get; set; } = new List<ProvinceService>();
    public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

public class ProvinceService
{
    public List<Hotel> Hotels { get; set; } = new List<Hotel>();
    public List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    public List<Entertainment> Entertainments { get; set; } = new List<Entertainment>();
    
}

public class Hotel
{
    public Guid DistrictId { get; set; }
    public int NumOfDay { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ServingQuantity { get; set; }
    public int NumOfRoom { get; set; }
    public HotelOptionRating? Option1 {  get; set; }
    public HotelOptionRating? Option2 {  get; set; }
    public HotelOptionRating? Option3 {  get; set; }
}

public class HotelOptionRating
{
    public Rating Rating { get; set; }
    public OptionClass OptionClass { get; set; }
}
public class Restaurant
{
    public Guid DistrictId { get; set; }
    public List<MenuQuotation> MenuQuotations { get; set; } = new List<MenuQuotation>();
}

public class MenuQuotation
{
    public DateTime Date { get; set; }
    public MenuOption? BreakfastOption1 { get; set; }
    public MenuOption? BreakfastOption2 { get; set; }
    public MenuOption? BreakfastOption3 { get; set; }
    public MenuOption? LunchOption1 { get; set; }
    public MenuOption? LunchOption2 { get; set; }
    public MenuOption? LunchOption3 { get; set; }
    public MenuOption? DinnerOption1 { get; set; }   
    public MenuOption? DinnerOption2 { get; set; }
    public MenuOption? DinnerOption3 { get; set; }


}

public class MenuOption
{
    public Guid MenuId { get; set; }
    public OptionClass OptionClass { get; set; }
}

public class Entertainment
{
    public Guid DistrictId { get; set; }
    public EntertainmentOption EntertainmentOption1 { get; set; }
    public EntertainmentOption EntertainmentOption2 { get; set; }
    public EntertainmentOption EntertainmentOption3 { get; set; }
}

public class EntertainmentOption
{
    public int QuantityLocation { get; set; }
    public OptionClass OptionClass { get; set; }
}

public class Vehicle
{
    public VehicleType VehicleType { get; set; }
    public Guid? StartPoint { get; set; }
    public Guid? StartPointDistrict { get; set; }
    public Guid? EndPoint { get; set; }
    public Guid? EndPointDistrict { get; set; }
    public int NumOfRentingDay { get; set; } = 1;
    public int NumOfVehicle { get; set; } = 1;
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
