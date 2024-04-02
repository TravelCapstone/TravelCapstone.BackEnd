using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TravelCapstone.BackEnd.API.Controllers;

public class MapInfo
{
    public class AddressComponent
    {
        [JsonProperty("long_name")] public string LongName { get; set; }

        [JsonProperty("short_name")] public string ShortName { get; set; }
    }

    public class Compound
    {
        [JsonProperty("district")] public string District { get; set; }

        [JsonProperty("commune")] public string Commune { get; set; }

        [JsonProperty("province")] public string Province { get; set; }
    }

    public class Geometry
    {
        [JsonProperty("location")] public Location Location { get; set; }

        [JsonProperty("boundary")] public object Boundary { get; set; }
    }

    public class Location
    {
        [JsonProperty("lat")] public double Lat { get; set; }

        [JsonProperty("lng")] public double Lng { get; set; }
    }

    public class PlusCode
    {
        [JsonProperty("compound_code")] public string CompoundCode { get; set; }

        [JsonProperty("global_code")] public string GlobalCode { get; set; }
    }

    public class Result
    {
        [JsonProperty("address_components")] public List<AddressComponent> AddressComponents { get; set; }

        [JsonProperty("formatted_address")] public string FormattedAddress { get; set; }

        [JsonProperty("geometry")] public Geometry Geometry { get; set; }

        [JsonProperty("place_id")] public string PlaceId { get; set; }

        [JsonProperty("reference")] public string Reference { get; set; }

        [JsonProperty("plus_code")] public PlusCode PlusCode { get; set; }

        [JsonProperty("compound")] public Compound Compound { get; set; }

        [JsonProperty("types")] public List<string> Types { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("address")] public string Address { get; set; }
    }

    public class Root
    {
        [JsonProperty("results")] public List<Result> Results { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}

[Route("api/[controller]")]
[ApiController]
public class MapController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;

    public MapController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpGet("geocode")]
    public async Task<IActionResult> Geocode(string address)
    {
        var client = _clientFactory.CreateClient();
        var apiKey = "mwH5JLtq3UHxo5PWN5iJmOPs41ft3EUWJIxNK9ad";
        var endpoint = $"https://rsapi.goong.io/Geocode?api_key={apiKey}&address={address}";

        var response = await client.GetAsync(endpoint);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<MapInfo.Root>(data);
            return Ok(obj);
        }

        return StatusCode((int)response.StatusCode, "Failed to fetch data from Goong API");
    }
}