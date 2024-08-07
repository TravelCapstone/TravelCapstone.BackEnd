﻿using TravelCapstone.BackEnd.Application;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Application.Services;
using TravelCapstone.BackEnd.Domain.Data;
using TravelCapstone.BackEnd.Infrastructure.Repositories;
using TravelCapstone.BackEnd.Infrastructure.UnitOfWork;

namespace TravelCapstone.BackEnd.API.Installers;

public class ServiceInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        //========//
        services.AddScoped<IDbContext, TravelCapstoneDbContext>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<IPrivateTourRequestService, PrivateTourRequestService>();
        services.AddScoped<IServiceProviderService, ServiceProviderService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IMapService, MapService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ISmsService, SmsService>();
        services.AddScoped<IServiceCostHistoryService, ServiceCostHistoryService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IFacilityServiceService, FacilityServiceService>();
        services.AddScoped<IDistrictService, DistrictService>();
     //   services.AddScoped<IFakeDataGenerator,FakeDataGenerator>();
        services.AddScoped<IAirportService, AirportService>();
        services.AddScoped<IReferenceTransportService, ReferenceTransportService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<ISellPriceHistoryService, SellPriceHistoryService>();
        services.AddScoped<ITourguideAssignmentsService, TourguideAssignmentsService>();
        services.AddScoped<IFacilityService, FacilityService>();
        services.AddScoped<IExcelService, ExcelService>();
        services.AddScoped<IDishService, DishService>();
        services.AddScoped<IMaterialService, MaterialService>();
        services.AddScoped<IFacilityTypeService, FacilityTypeService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IDriverService, DriverService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IHumanResourceFeeService, HumanResourceFeeService>();
        services.AddScoped<IDriverService, DriverService>(); 
        services.AddScoped<IAssuranceService, AssuranceService>();
        services.AddScoped<IManageFeeReferenceService, ManageFeeReferenceService>();
        services.AddScoped<IFacilityRatingService, FacilityRatingService>();
		services.AddScoped<IConfigurationService, ConfigurationService>();
    }
}