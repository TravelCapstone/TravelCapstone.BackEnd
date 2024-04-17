using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IFakeDataGenerator
    {
        public Task<List<ServiceProvider>> GenerateServiceProviders(int count);
        public Task<List<ServiceRating>> GenerateServiceRatings(int count);
        public Task<List<Service>> GenerateServices(List<ServiceProvider> serviceProviders, List<ServiceRating> serviceRatings, int count);
        public Task<List<ServiceCostHistory>> GenerateServiceCostHistories(List<Service> services, int count);
        public Task<List<SellPriceHistory>> GenerateSellPriceHistories(List<Service> services, int count);
        public  Task<List<Tour>> GenerateFakeTours(int count);
        public Task<List<Route>> GenerateFakeRoutes(int count, List<DayPlan> dayPlans, List<Destination> destinations);
        public Task<List<Destination>> GenerateFakeDestinations(int count);
        public Task<List<DayPlan>> GenerateFakeDayPlans(List<Tour> tours, int countPerTour);





    }
}
