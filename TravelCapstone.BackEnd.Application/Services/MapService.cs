using Newtonsoft.Json;
using RestSharp;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using Prediction = TravelCapstone.BackEnd.Common.DTO.ProcessDTO.Prediction;

namespace TravelCapstone.BackEnd.Application.Services;

public class MapService : GenericBackendService, IMapService
{
    public const string APIKEY = "mwH5JLtq3UHxo5PWN5iJmOPs41ft3EUWJIxNK9ad";

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
}