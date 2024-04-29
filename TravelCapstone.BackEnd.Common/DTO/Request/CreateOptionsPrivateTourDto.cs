using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Request;

public class CreateOptionsPrivateTourDto
{
    public OptionClass OptionClass { get; set; }
    public Guid PrivateTourRequestId { get; set; }  
    public List<Location> Locations { get; set; } = new List<Location>();
    public List<Vehicle> Vehicles { get; set; }= new List<Vehicle>();

}


public class Hotel
{
    public int NumOfDay { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public  Rating Rating { get; set; }
    public int ServingQuantity { get; set; }
}

public class Location
{
    public Guid DistrictId { get; set; }
    public List<Hotel> Hotels { get; set; } = new List<Hotel>();
    public List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    public Entertainment Entertainment { get; set; } = null!;  
}

public class Restaurant
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Rating Rating { get; set; }
    public int MealPerDay { get; set; } = 1;
}
public class Entertainment
{
    public int QuantityLocation { get; set; }
}

public class Vehicle
{
    public VehicleType VehicleType { get; set; }
    public Guid? StartPoint { get; set; }
    public Guid? EndPoint { get; set; }
    public int NumOfRentingDay { get; set; } = 1;
}
