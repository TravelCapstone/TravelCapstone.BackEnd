using System;
using System.Collections.Generic;
using System.Linq;
using Faker;
using NPOI.SS.Formula.Functions;
using TravelCapstone.BackEnd.Application;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;
using Bogus;
using TravelCapstone.BackEnd.Application.Services;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

public class FakeDataGenerator : GenericBackendService, IFakeDataGenerator
{

    private IRepository<ServiceProvider> _serviceProviderRepository;
    private IRepository<ServiceRating> _serviceRatingRepository;
    private IRepository<Service> _serviceRepository;
    private IRepository<ServiceCostHistory> _serviceCostHistoryRepository;
    private IRepository<SellPriceHistory> _sellPriceHistoryRepository;
    private IRepository<Commune> _communeRepository;
    private IUnitOfWork _unitOfWork;

    public FakeDataGenerator(IRepository<ServiceProvider> serviceProviderRepository, IRepository<ServiceRating> serviceRatingRepository, IRepository<Service> serviceRepository, IRepository<ServiceCostHistory> serviceCostHistoryRepository, IRepository<SellPriceHistory> sellPriceHistoryRepository, IUnitOfWork unitOfWork, IRepository<Commune> communeRepository, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _serviceProviderRepository = serviceProviderRepository;
        _serviceRatingRepository = serviceRatingRepository;
        _serviceRepository = serviceRepository;
        _serviceCostHistoryRepository = serviceCostHistoryRepository;
        _sellPriceHistoryRepository = sellPriceHistoryRepository;
        _unitOfWork = unitOfWork;
        _communeRepository = communeRepository;
    }

    public async Task<List<ServiceProvider>> GenerateServiceProviders(int count)
    {
        var serviceProviders = new List<ServiceProvider>();
        for (var i = 0; i < count; i++)
        {
            var serviceProvider = new ServiceProvider
            {
                Id = Guid.NewGuid(),
                Name = Name.FullName()
            };
            serviceProviders.Add(serviceProvider);

        }
        await _serviceProviderRepository.InsertRange(serviceProviders);
        await _unitOfWork.SaveChangesAsync();
        return serviceProviders;
    }

    public async Task<List<ServiceRating>> GenerateServiceRatings(int count)
    {
        var serviceRatings = new List<ServiceRating>();
        var random = new Random();
        for (var i = 0; i < count; i++)
        {
            var serviceRating = new ServiceRating
            {
                Id = Guid.NewGuid(),
                ServiceTypeId = (TravelCapstone.BackEnd.Domain.Enum.ServiceType)random.Next(System.Enum.GetValues(typeof(TravelCapstone.BackEnd.Domain.Enum.ServiceType)).Length),
                Rating = random.Next(0, 4)
            };
            serviceRatings.Add(serviceRating);

        }
        await _serviceRatingRepository.InsertRange(serviceRatings);
        await _unitOfWork.SaveChangesAsync();
        return serviceRatings;
    }

    public async Task<List<Service>> GenerateServices(List<ServiceProvider> serviceProviders, List<ServiceRating> serviceRatings , int count)
    {
        var services = new List<Service>();
        var random = new Random();
        var communes = await _communeRepository.GetAllDataByExpression(null, 0, 0, null); // Đây là phương thức bạn cần triển khai trong repository của Commune

        for (var i = 0; i < count; i++)
        {
            var serviceProvider = serviceProviders[random.Next(serviceProviders.Count)];
            var serviceRating = serviceRatings[random.Next(serviceRatings.Count)];
            var service = new Service
            {
                Id = Guid.NewGuid(),
                Name = Faker.Company.Name(),
                Description = Lorem.Paragraph(),
                //Type = (TravelCapstone.BackEnd.Domain.Enum.ServiceType)random.Next(System.Enum.GetValues(typeof(TravelCapstone.BackEnd.Domain.Enum.ServiceType)).Length),
                IsActive = true,
                Address = Faker.Address.StreetAddress(),
                CommunceId = communes.Items[random.Next(communes.Items.Count)].Id, // Chọn một Commune ngẫu nhiên từ danh sách
                ServiceProviderId = serviceProvider.Id,
                ServiceRatingId = serviceRating.Id
            };
            services.Add(service);

        }
        await _serviceRepository.InsertRange(services);
        await _unitOfWork.SaveChangesAsync();
        return services;
    }

    public async Task<List<ServiceCostHistory>> GenerateServiceCostHistories(List<Service> services, int count)
    {
        var serviceCostHistories = new List<ServiceCostHistory>();
        var random = new Random();
        foreach (var service in services)
        {
            for (var i = 0; i < count; i++)
            {
                var serviceCostHistory = new ServiceCostHistory
                {
                    Id = Guid.NewGuid(),
                    PricePerAdult = Convert.ToDouble(random.Next(50, 500)),
                    PricePerChild = Convert.ToDouble(random.Next(20, 250)),
                    MOQ = random.Next(1, 100),
                    UnitId = (TravelCapstone.BackEnd.Domain.Enum.Unit)random.Next(System.Enum.GetValues(typeof(TravelCapstone.BackEnd.Domain.Enum.Unit)).Length),
                    //    Unit = Unit.PERSON,
                    Date = DateTime.Now,
                    ServiceId = service.Id
                };
                serviceCostHistories.Add(serviceCostHistory);

            }
        }
        await _serviceCostHistoryRepository.InsertRange(serviceCostHistories);
        await _unitOfWork.SaveChangesAsync();
        return serviceCostHistories;
    }

    public async Task<List<SellPriceHistory>> GenerateSellPriceHistories(List<Service> services, int count)
    {
        var sellPriceHistories = new List<SellPriceHistory>();
        var random = new Random();
        foreach (var service in services)
        {
            for (var i = 0; i < count; i++)
            {
                var sellPriceHistory = new SellPriceHistory
                {
                    Id = Guid.NewGuid(),
                    PricePerAdult = Convert.ToDouble(random.Next(50, 500)),
                    PricePerChild = Convert.ToDouble(random.Next(20, 250)),
                    MOQ = random.Next(1, 100),
                    Date = DateTime.Now,
                    ServiceId = service.Id
                };
                sellPriceHistories.Add(sellPriceHistory);
            }
        }
        await _sellPriceHistoryRepository.InsertRange(sellPriceHistories);
        await _unitOfWork.SaveChangesAsync();
        return sellPriceHistories;
    }


    public async Task<List<Tour>> GenerateFakeTours(int count)
    {
        var tourRepository = Resolve<IRepository<Tour>>();
        var tours = new List<Tour>();
        var random = new Random();
        var communes = await _communeRepository.GetAllDataByExpression(null, 0, 0, null); // Triển khai phương thức GetAllDataByExpression trong CommuneRepository

        for (var i = 0; i < count; i++)
        {
            var tour = new Tour
            {
                Id = Guid.NewGuid(),
                Name = Faker.Company.Name(),
                Description = Lorem.Paragraph(),
                VehicleTypeId = (VehicleType)random.Next(System.Enum.GetValues(typeof(VehicleType)).Length),
                TotalPrice = random.NextDouble() * 1000,
                PricePerAdult = random.NextDouble() * 500,
                PricePerChild = random.NextDouble() * 200,
                StartDate = DateTime.Now.AddDays(random.Next(30)), // Random date within next 30 days
                EndDate = DateTime.Now.AddDays(random.Next(31, 60)), // Random date between 31st and 60th day from now
                TourTypeId = (TourType)random.Next(System.Enum.GetValues(typeof(TourType)).Length),
                QRCode = null,
                TourStatusId = (TourStatus)random.Next(System.Enum.GetValues(typeof(TourStatus)).Length),
                BasedOnTourId = null, // Cần điều chỉnh nếu cần thiết
                TourGuideId = "e392d0b3-1ffe-4bfe-81cb-c3f80c402ea5"
            };
            tours.Add(tour);
        }
        await tourRepository!.InsertRange(tours);
        await _unitOfWork.SaveChangesAsync();
        return tours;
    }
    public async Task<List<DayPlan>> GenerateFakeDayPlans(List<Tour> tours, int countPerTour)
    {
        var dayPlanRepository = Resolve<IRepository<DayPlan>>();
        var dayPlans = new List<DayPlan>();
        var random = new Random();

        foreach (var tour in tours)
        {
            for (int i = 0; i < countPerTour; i++)
            {
                var dayPlan = new DayPlan
                {
                    Id = Guid.NewGuid(),
                    Name = $"{tour.Name} - Day {i + 1}",
                    Description = $"Day plan for {tour.Name} - Day {i + 1}",
                    TourId = tour.Id
                };

                dayPlans.Add(dayPlan);
            }
        }
       await dayPlanRepository!.InsertRange(dayPlans);
        await _unitOfWork.SaveChangesAsync();
        // Save to database or return the list, depending on your needs
        return dayPlans;
    }

    public async Task<List<Route>> GenerateFakeRoutes(int count, List<DayPlan> dayPlans, List<Destination> destinations)
    {
        var repository = Resolve<IRepository<Route>>();
        var routes = new List<Route>();
        var random = new Random();

        for (var i = 0; i < count; i++)
        {
            var route = new Route()
            {
                Id = Guid.NewGuid(),
                Name = Faker.Company.Name(),
                Note = Lorem.Sentence(),
                StartTime = DateTime.Now.AddDays(random.Next(30)),
                EndTime = DateTime.Now.AddDays(random.Next(31, 60)),
                DayPlanId = dayPlans[random.Next(dayPlans.Count)].Id,
                StartPointId = destinations[random.Next(destinations.Count)].Id,
                EndPointId = destinations[random.Next(destinations.Count)].Id,
                ParentRouteId = null // Cần điều chỉnh nếu cần thiết
            };
            routes.Add(route);
        }
        await repository!.InsertRange(routes);
        await _unitOfWork.SaveChangesAsync();
        return routes;
    }

    public async Task<List<Destination>> GenerateFakeDestinations(int count)
    {
        var repository =Resolve<IRepository<Destination>>();
        var communeRepository = Resolve<IRepository<Commune>>();

        var destinations = new List<Destination>();
        var random = new Random();
       var list = await communeRepository!.GetAllDataByExpression(null, 0, 0, null);
        var communes= list.Items;
        for (var i = 0; i < count; i++)
        {
            var commune = communes[random.Next(communes.Count)];

            var destination = new Destination
            {
                Id = Guid.NewGuid(),
                Name = Faker.Address.City(),
                CommunceId = commune.Id
            };
            destinations.Add(destination);
        }
        await repository!.InsertRange(destinations);
        await _unitOfWork.SaveChangesAsync();
        return destinations;
    }
}
