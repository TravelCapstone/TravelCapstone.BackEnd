using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography.Xml;
using System.Text;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;
using static System.Net.WebRequestMethods;
using Prediction = TravelCapstone.BackEnd.Common.DTO.ProcessDTO.Prediction;

namespace TravelCapstone.BackEnd.Application.Services;

public class MapService : GenericBackendService, IMapService
{
    public const string APIKEY = "AFSFy0pKHHUhZ89JFJhQ6AJMjPXEcMcKyzVnGj7L";
    private IUnitOfWork _unitOfWork;

    public MapService(IUnitOfWork unitOfWork ,IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AppActionResult> Geocode(string address)
    {
        AppActionResult result = new();
        try
        {
            var endpoint = $"https://rsapi.goong.io/Geocode?api_key={APIKEY}&address={address}";
            var client = new RestClient();
            var request = new RestRequest(endpoint);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content;
                var obj = JsonConvert.DeserializeObject<MapInfo.Root>(data!);
                result.Result = obj;
            }
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra khi sử dụng API với GoongMap {e.Message} ");
        }

        return result;
    }

    public async Task<AppActionResult> AutoComplete(string address)
    {
        AppActionResult result = new();

        try
        {
            var endpoint = $"https://rsapi.goong.io/Place/AutoComplete?api_key={APIKEY}&input={address}";
            var client = new RestClient();
            var request = new RestRequest(endpoint);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content;
                var obj = JsonConvert.DeserializeObject<Root>(data!);
                var selectedData = obj.Predictions.Select(p => new Prediction
                {
                    Description = p.Description,
                    Compound = p.Compound
                }).ToList();
                result.Result = selectedData;
            }
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra khi sử dụng API với GoongMap {e.Message} ");
        }
        return result;
    }

    public async Task<AppActionResult> GetVehicle(Guid startPoint, Guid endPoint)
    {
        AppActionResult result = new AppActionResult();
        try
        {
            var provinceRepository = Resolve<IRepository<Province>>();
            var referenceTransportPriceRepository = Resolve<IRepository<ReferenceTransportPrice>>();
            var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
            var startProvince = await provinceRepository!.GetById(startPoint);
            var endProvince = await provinceRepository!.GetById(endPoint);
            if (startProvince == null || endProvince == null)
            {
                result = BuildAppActionResultError(result, $"Tỉnh bắt đầu hoặc tỉnh kết thúc không tồn tại");
            }
            if (!BuildAppActionResultIsError(result))
            {
                List<EstimatedVehiclePrice> estimatedVehiclePrices = new List<EstimatedVehiclePrice>();

                var listFlight = await referenceTransportPriceRepository!.GetAllDataByExpression
                        (a => (a.Departure!.PortType == Domain.Enum.PortType.AIRPORT
                        && a.Arrival!.PortType == Domain.Enum.PortType.AIRPORT &&
                        a.Departure!.Commune!.District!.ProvinceId == startPoint
                        && a.Arrival!.Commune!.District!.ProvinceId == endPoint)
                        ||
                         a.Arrival!.PortType == Domain.Enum.PortType.AIRPORT
                         && a.Departure!.PortType == Domain.Enum.PortType.AIRPORT &&
                        a.Arrival!.Commune!.District!.ProvinceId == endPoint
                        && a.Departure!.Commune!.District!.ProvinceId == startPoint
                        , 0, 0, null);

                var listCoach = await sellPriceRepository!.GetAllDataByExpression
                        (a => a.FacilityService.ServiceTypeId == ServiceType.VEHICLE,
                        0, 0, null);
                if (listCoach.Items!.Any())
                {
                    estimatedVehiclePrices.Add(new EstimatedVehiclePrice
                    {
                        Min = listCoach.Items!.Min(a => a.Price),
                        Max = listCoach.Items!.Max(a => a.Price),
                        Vehicle = VehicleType.COACH
                    });
                }

                if (listFlight.Items!.Any())
                {
                    estimatedVehiclePrices.Add(new EstimatedVehiclePrice()
                    {
                        Min = listFlight.Items!.Min(a => a.AdultPrice),
                        Max = listFlight.Items!.Max(a => a.AdultPrice),
                        Vehicle = VehicleType.PLANE
                    });

                }

            }

        }
        catch (Exception e)
        {


        }
        return result;
    }

    public async Task<AppActionResult> ImportPositionToProvince()
    {
        AppActionResult result = new AppActionResult();
        try
        {
            var provinceRepository = Resolve<IRepository<Province>>();
            var provinceDb = await provinceRepository!.GetAllDataByExpression(null, 0, 0, null, false, null);
            MapInfo.Root position = null;
            foreach( var province in provinceDb.Items! ) {
                if(province.lng == null || province.lat == null)
                {
                    position = (await Geocode(province.Name)).Result as MapInfo.Root;
                    if(position != null)
                    {
                        province.lat = position.Results[0].Geometry.Location.Lat;
                        province.lng = position.Results[0].Geometry.Location.Lng;
                    }
                    await provinceRepository.Update(province);
                }
            }
            await _unitOfWork.SaveChangesAsync();

        }catch (Exception e)
        {
            result = BuildAppActionResultError(result, e.Message);
        }
        return result;
    }

    public async Task<AppActionResult> FindOptimalPath(Guid StartDestinationId, List<Guid> DestinationIds, bool IsPilgrimageTrip)
    {
        AppActionResult result = new AppActionResult();
        try
        {
            var provinceRepository = Resolve<IRepository<Province>>();
            var startProvince = await provinceRepository!.GetByExpression(p => p.Id == StartDestinationId, null);
            if(startProvince == null )
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy thông tin địa điểm bắt đầu với id {StartDestinationId}");
                return result;
            }

            var onWayProvinces = await provinceRepository!.GetAllDataByExpression(p => DestinationIds.Contains(p.Id),0,0,null,false, null);
            if(onWayProvinces.Items!.Count != DestinationIds.Count)
            {
                foreach(var item in onWayProvinces.Items)
                {
                    if (!DestinationIds.Contains(item.Id))
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy thông tin địa điểm với id {item.Id}");
                        return result;
                    }
                }
            }

            if (true)
            {
                string start = $"{startProvince.lat.ToString()},{startProvince.lng.ToString()}";
                StringBuilder waypoints = new StringBuilder();
                foreach(var item in onWayProvinces.Items)
                {
                    waypoints.Append($"{item.lat.ToString()},{item.lng.ToString()};");
                }
                waypoints.Remove(waypoints.Length -1, 1);
                string endpoint = $"https://rsapi.goong.io/trip?origin={start}&waypoints={waypoints.ToString()}&api_key={APIKEY}";
                var client = new RestClient();
                var request = new RestRequest(endpoint);

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content;
                    var obj = JsonConvert.DeserializeObject<TripInfo.Root>(data!);
                    OptimalTripResponseDTO optimalTripResponseDTO = new OptimalTripResponseDTO();
                    optimalTripResponseDTO.TotalDistance = obj.Trips[0].Distance;
                    optimalTripResponseDTO.TotalDuration = obj.Trips[0].Duration;
                    string placeEndPoint = null;
                    TripInfo.Leg currentTrip = null;
                    TripInfo.Waypoint currentWaypoint = null;
                    List<RestResponse> provinceResponse = new List<RestResponse>();

                    obj.Waypoints = obj.Waypoints.OrderBy(o => o.WaypointIndex).ToList();

                    for(int i = 0; i < obj.Trips[0].Legs.Count; i++)
                    {
                        placeEndPoint = $"https://rsapi.goong.io/Place/Detail?place_id={obj.Waypoints[i].PlaceId}&api_key={APIKEY}";
                        response = await client.ExecuteAsync(new RestRequest(placeEndPoint));
                        provinceResponse.Add(response);
                        await Task.Delay(200);
                    }
                    string currentProvinceName = null;

                    for(int i = 0; i < obj.Trips[0].Legs.Count; i++)
                    {
                        currentTrip = obj.Trips[0].Legs[i];
                        currentWaypoint = obj.Waypoints[i];
                        response = provinceResponse[i];
                        LocationResultDTO.GeocodeResponse location;
                        if(response.IsSuccessStatusCode)
                        {
                            location = JsonConvert.DeserializeObject<LocationResultDTO.GeocodeResponse>(response.Content!)!;
                            currentProvinceName = location.Result.FormattedAddress.Split(", ")[location.Result.FormattedAddress.Split(", ").Length - 1];
                            if (currentProvinceName.Contains(startProvince.Name) || startProvince.Name.Contains(currentProvinceName))
                            {
                                optimalTripResponseDTO.OptimalTrip!.Add(new RouteNode
                                {
                                    Index = currentWaypoint.WaypointIndex,
                                    ProvinceId = startProvince.Id,
                                    ProvinceName = startProvince.Name,
                                    VehicleToNextDestination = VehicleType.BUS,
                                    DistanceToNextDestination = currentTrip.Distance,
                                    Duration = currentTrip.Duration,
                                });
                                continue;
                            }
                            var province = onWayProvinces.Items.FirstOrDefault(p => currentProvinceName.Contains(p.Name) || p.Name.Contains(currentProvinceName));
                            if (province != null)
                            {
                                optimalTripResponseDTO.OptimalTrip!.Add(new RouteNode
                                {
                                    Index= currentWaypoint.WaypointIndex,
                                    ProvinceId = province.Id,
                                    ProvinceName = province.Name,
                                    VehicleToNextDestination = VehicleType.BUS,
                                    DistanceToNextDestination = currentTrip.Distance,
                                    Duration = currentTrip.Duration,
                                });
                            }
                        }
                    }
                 
                    result.Result = optimalTripResponseDTO.OptimalTrip.OrderBy(o => o.Index);
                }
            }



        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, e.Message);
        }
        return result;
    }

    public async Task<AppActionResult> GetEstimateTripDate(Guid StartDestinationId, List<Guid> DestinationIds, VehicleType vehicleType, DateTime startDate, DateTime endDate)
    {
        AppActionResult result = new AppActionResult();
        try 
        {
            var provinceRepository = Resolve<IRepository<Province>>();
            var startProvince = await provinceRepository!.GetByExpression(p => p.Id == StartDestinationId, null);
            if (startProvince == null)
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy thông tin địa điểm bắt đầu với id {StartDestinationId}");
                return result;
            }
            var onWayProvinces = await provinceRepository!.GetAllDataByExpression(p => DestinationIds.Contains(p.Id), 0, 0, null, false, null);
            if (onWayProvinces.Items!.Count != DestinationIds.Count)
            {
                foreach (var item in onWayProvinces.Items)
                {
                    if (!DestinationIds.Contains(item.Id))
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy thông tin địa điểm với id {item.Id}");
                        return result;
                    }
                }
            }

            string start = $"{startProvince.lat.ToString()}, {startProvince.lng.ToString()}";
            StringBuilder waypoints = new StringBuilder();
            foreach (var item in onWayProvinces.Items)
            {
                waypoints.Append($"{item.lat.ToString()},{item.lng.ToString()};");
            }
            waypoints.Remove(waypoints.Length - 1, 1);
            string vehicle = vehicleType.ToString().ToLower();
            string endpoint = $"https://rsapi.goong.io/DistanceMatrix?origins={start}&destinations={waypoints.ToString()}&vehicle={vehicle}&api_key={APIKEY}";
            var client = new RestClient();
            var request = new RestRequest(endpoint);

            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content;
                var obj = JsonConvert.DeserializeObject<DistanceTripInfo.Root>(data!);
                if (obj != null && obj.Rows != null && obj.Rows.Count > 0 && obj.Rows[0].Elements != null && obj.Rows[0].Elements.Count > 0)
                {
                    DistanceTripResponseDto distanceTripResponseDto = new DistanceTripResponseDto();
                    distanceTripResponseDto.DistanceInText = obj.Rows[0].Elements[0].Distance.Distances;
                    distanceTripResponseDto.DurationInText = obj.Rows[0].Elements[0].Duration.Durations;
                    distanceTripResponseDto.VehicleType = vehicleType;

                    int estimateTimeOfTrip = obj.Rows[0].Elements[0].Duration.NumberOfTime;

                    double estimateTimeOfTripInHours = estimateTimeOfTrip / 3600;
                        
                    DateTime dateTravel = startDate.AddHours(estimateTimeOfTripInHours);

                    (int numberOfDayToTravel, int numberOfNightToTravel) = CalculateTimesToCheckOut(startDate, dateTravel);

                    (int numberOfDaysOfTrip, int NumberOfNightOfTrip) = CalculateDaysAndNights(startDate, endDate);

                    int numOfDayRemain = numberOfDaysOfTrip - numberOfDayToTravel;
                    int numOfNightRemain = NumberOfNightOfTrip - numberOfNightToTravel;

                    distanceTripResponseDto.NumOfDay = numOfDayRemain;
                    distanceTripResponseDto.NumOfNight = numOfNightRemain;  

                    result.Result = distanceTripResponseDto;        
                }
            }
            else
            {
                result = BuildAppActionResultError(result ,"Kết nối tới API Goong không được");
            }
        } 
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, e.Message);
        }
        return result;
    }

    private static (int numberOfDays, int numberOfNights) CalculateTimesToCheckOut(DateTime startDate, DateTime endDate)
    {
        int numberOfDays, numberOfNights;

        // Kiểm tra nếu end_date lớn hơn hoặc bằng 14:00
        if (endDate.Hour >= 14)
        {
            // Trường hợp này chỉ được tính là một đêm
            numberOfDays = 0;
            numberOfNights = 1;
        }
        else
        {
            // Trường hợp này chỉ được tính là một ngày
            numberOfDays = 1;
            numberOfNights = 0;
        }

        return (numberOfDays, numberOfNights);
    }


    private (int numberOfDays, int numberOfNights) CalculateDaysAndNights(DateTime startDate, DateTime endDate)
    {
        // Check if endDate is after startDate
        if (endDate <= startDate)
        {
            // If endDate is not after startDate, return 0 days and 0 nights
            return (0, 0);
        }

        // Calculate the total number of days
        int numberOfDays = (int)(endDate.Date - startDate.Date).TotalDays;

        // Calculate the number of nights
        int numberOfNights = numberOfDays;

        // Check if endDate's time is after the startDate's time
        if (endDate.TimeOfDay > startDate.TimeOfDay)
        {
            // Add an extra night if endDate's time is past startDate's time
            numberOfNights++;
        }
        else if (endDate.TimeOfDay < startDate.TimeOfDay)
        {
            // Subtract a night if endDate's time is before startDate's time
            numberOfNights--;
        }

        return (numberOfDays, numberOfNights);
    }
}