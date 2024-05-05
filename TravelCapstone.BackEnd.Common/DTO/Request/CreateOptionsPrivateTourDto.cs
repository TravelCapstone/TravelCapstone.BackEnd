using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Request;

public class CreateOptionsPrivateTourDto
{
    public OptionClass OptionClass { get; set; }
    public Guid PrivateTourRequestId { get; set; }
    public List<Hotel> Hotels { get; set; } = new List<Hotel>();
    public List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    public List<Entertainment> Entertainments { get; set; } = new List<Entertainment>();
    public List<Vehicle> Vehicles { get; set; }= new List<Vehicle>();

}


public class Hotel
{
    public Guid DistrictId { get; set; }
    public int NumOfDay { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public  Rating Rating { get; set; }
    public int ServingQuantity { get; set; }
    public int NumOfRoom { get; set; }
}

public class Restaurant
{
    public Guid DistrictId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Rating Rating { get; set; }
    public int MealPerDay { get; set; } = 1;
    public int NumOfDay { get; set; } = 1;
    public int ServingQuantity { get; set; } = 10;
    public ServiceAvailability ServiceAvailability { get; set; } = ServiceAvailability.BOTH;
}
public class Entertainment
{
    public Guid DistrictId { get; set; }
    public int QuantityLocation { get; set; }
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
