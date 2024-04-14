using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakeDataController : ControllerBase
    {
        private IFakeDataGenerator _fakeDataGenerator;

        public FakeDataController(IFakeDataGenerator fakeDataGenerator)
        {
            _fakeDataGenerator = fakeDataGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var serviceProviders = await _fakeDataGenerator.GenerateServiceProviders(50);
            var serviceRatings = await _fakeDataGenerator.GenerateServiceRatings(50);
            var services = await _fakeDataGenerator.GenerateServices(serviceProviders, serviceRatings, 50);
            var serviceCostHistories = await _fakeDataGenerator.GenerateServiceCostHistories(services, 50);
            var sellPriceHistories = await _fakeDataGenerator.GenerateSellPriceHistories(services, 50);
            var tours = await _fakeDataGenerator.GenerateFakeTours(50);
            var dayPlan = await _fakeDataGenerator.GenerateFakeDayPlans(tours, 3);
            var destination = await _fakeDataGenerator.GenerateFakeDestinations(50);
            var route = await _fakeDataGenerator.GenerateFakeRoutes(50, dayPlan, destination);
            return Ok();
        }
    }
}
