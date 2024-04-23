using Newtonsoft.Json;
using RestSharp;
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

    public MapService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
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
                        (a => a.Service!.ServiceRating!.ServiceTypeId == ServiceType.VEHICLE_SUPPLY,
                        0, 0, null);
                if (listCoach.Items!.Any())
                {
                    estimatedVehiclePrices.Add(new EstimatedVehiclePrice
                    {
                        Min = listCoach.Items!.Min(a=> a.Price),
                        Max = listCoach.Items!.Max(a=> a.Price),
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
}