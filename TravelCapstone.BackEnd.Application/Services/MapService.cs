using Newtonsoft.Json;
using RestSharp;
using System.Text;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;
using Prediction = TravelCapstone.BackEnd.Common.DTO.ProcessDTO.Prediction;

namespace TravelCapstone.BackEnd.Application.Services;

public class MapService : GenericBackendService, IMapService
{
    public const string APIKEY = "59ShUUOfSsJVDcwkynlpIKDuUSs0djQJpZo7Oixy";
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

            if (IsPilgrimageTrip)
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
                    for(int i = 0; i < obj.Trips[0].Legs.Count; i++)
                    {
                        currentTrip = obj.Trips[0].Legs[i];
                        currentWaypoint = obj.Waypoints[i];
                        placeEndPoint = $"https://rsapi.goong.io/Place/Detail?place_id={currentWaypoint.PlaceId}&api_key={APIKEY}";
                        response = await client.ExecuteAsync(new RestRequest(placeEndPoint));
                        LocationResultDTO.GeocodeResponse location;
                        if(response.IsSuccessStatusCode)
                        {
                            location = JsonConvert.DeserializeObject<LocationResultDTO.GeocodeResponse>(response.Content!)!;
                            if (location.Result.FormattedAddress.Contains(startProvince.Name))
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
                            var province = onWayProvinces.Items.FirstOrDefault(p => location.Result.FormattedAddress.Contains(p.Name));
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
                 
                    result.Result = optimalTripResponseDTO;
                }
            }



        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, e.Message);
        }
        return result;
    }
}