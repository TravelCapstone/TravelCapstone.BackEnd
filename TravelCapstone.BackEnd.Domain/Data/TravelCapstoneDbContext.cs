using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Domain.Data;

public class TravelCapstoneDbContext : IdentityDbContext<Account>, IDbContext
{
    public TravelCapstoneDbContext()
    {
    }

    public TravelCapstoneDbContext(DbContextOptions options) : base(options)
    {
    }
    #region DBSet
    #region model
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<TourTraveller> TourTravellers { get; set; }
    public DbSet<AttendanceDetail> AttendanceDetails { get; set; }
    public DbSet<AttendanceRoute> AttendanceRoutes { get; set; }
    public DbSet<Commune> Communes { get; set; }
    public DbSet<DayPlan> DayPlans { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialAssignment> MaterialAssignments { get; set; }
    public DbSet<OptionQuotation> OptionQuotations { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<PlanServiceCostDetail> PlanServiceCostDetails { get; set; }
    public DbSet<PrivateJoinTourRequest> PrivateJoinTourRequests { get; set; }
    public DbSet<RequestedLocation> RequestedLocations { get; set; }
    public DbSet<PrivateTourRequest> PrivateTourRequests { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<QuotationDetail> QuotationDetails { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<SellPriceHistory> SellPriceHistorys { get; set; }
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<FacilityService> FacilityServices { get; set; }

    public DbSet<ServiceCostHistory> ServiceCostHistorys { get; set; }
    public DbSet<ServiceProvider> ServiceProviders { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourRegistration> TourRegistrations { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleRoute> VehicleRoutes { get; set; }
    public DbSet<VehicleQuotationDetail> VehicleQuotationDetails { get; set; }
    public DbSet<Port> Ports { get; set; }
    public DbSet<FacilityRating> FacilityRatings { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuDish> MenuDishes { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<TourguideQuotationDetail> TourguideQuotationDetails { get; set; }
    public DbSet<TourguideAssignment> TourguideAssignments { get; set; }
    public DbSet<TransportServiceDetail> TransportServiceDetails { get; set; }
    public DbSet<ReferenceTransportPrice> ReferenceTransportPrices { get; set; }
    public DbSet<TourguideScope> TourguideScopes { get; set; }
    public DbSet<TourGuideSalaryHistory> TourGuideSalaryHistories { get; set; }
    public DbSet<DriverSalaryHistory> DriverSalaryHistories { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventDetail> EventDetails { get; set; }
    public DbSet<EventDetailPriceHistory> EventDetailPriceHistories { get; set; }
    public DbSet<Assurance> Assurances { get; set; }
    public DbSet<AssurancePriceHistory> AssurancePriceHistories { get; set; }
    public DbSet<MaterialPriceHistory> MaterialPriceHistories { get; set; }
    public DbSet<OptionEvent> OptionEvents { get; set; }
    public DbSet<ManagementFeeReference> ManagementFeeReferences { get; set; }
    public DbSet<FamilyDetailRequest> FamilyDetailRequests { get; set; }
    public DbSet<StaticFile> StaticFiles { get; set; }
    public DbSet<TourRating> TourRatings { get; set; }

	#endregion

	#region Enum
	public DbSet<Models.EnumModels.AttendanceRouteType> AttendanceRouteTypes { get; set; }
    public DbSet<Models.EnumModels.MealType> MealTypes { get; set; }
    public DbSet<Models.EnumModels.ServiceType> ServiceTypes { get; set; }
    public DbSet<Models.EnumModels.AttendanceType> AttendanceTypes { get; set; }
    public DbSet<Models.EnumModels.JoinTourStatus> JoinTourStatuses { get; set; }
    public DbSet<Models.EnumModels.MaterialType> MaterialTypes { get; set; }
    public DbSet<Models.EnumModels.OptionClass> OptionClasses { get; set; }
    public DbSet<Models.EnumModels.OptionQuotationStatus> OptionQuotationStatuses { get; set; }
    public DbSet<Models.EnumModels.OrderStatus> OrderStatuses { get; set; }
    public DbSet<Models.EnumModels.PrivateTourStatus> PrivateTourStatuses { get; set; }
    public DbSet<Models.EnumModels.FacilityType> FacilityTypes { get; set; }
    public DbSet<Models.EnumModels.TourStatus> TourStatuses { get; set; }
    public DbSet<Models.EnumModels.Rating> Ratings { get; set; }
    public DbSet<Models.EnumModels.TourType> TourTypes { get; set; }
    public DbSet<Models.EnumModels.TransactionType> TransactionTypes { get; set; }
    public DbSet<Models.EnumModels.Unit> Units { get; set; }
    public DbSet<Models.EnumModels.VehicleType> VehicleTypes { get; set; }
    public DbSet<Models.EnumModels.PortType> PortTypes { get; set; }
    public DbSet<Models.EnumModels.ContractStatus> ContractStatuses { get; set; }
    public DbSet<Models.EnumModels.ServiceAvailability> ServiceAvailabilities { get; set; }
    public DbSet<Models.EnumModels.DishType> DishTypes { get; set; }
    public DbSet<Models.EnumModels.DietaryPreference> DietaryPreferences { get; set; }
    public DbSet<Models.EnumModels.ManagementFeeType> ManagementFeeTypes { get; set; }
    #endregion
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "6a32e12a-60b5-4d93-8306-82231e1232d7",
            Name = "ADMIN",
            ConcurrencyStamp = "6a32e12a-60b5-4d93-8306-82231e1232d7",
            NormalizedName = "admin"
        });
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "85b6791c-49d8-4a61-ad0b-8274ec27e764",
            Name = "STAFF",
            ConcurrencyStamp = "85b6791c-49d8-4a61-ad0b-8274ec27e764",
            NormalizedName = "staff"
        });
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "814f9270-78f5-4503-b7d3-0c567e5812ba",
            Name = "TOUR GUIDE",
            ConcurrencyStamp = "814f9270-78f5-4503-b7d3-0c567e5812ba",
            NormalizedName = "tour guide"
        });
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "02962efa-1273-46c0-b103-7167b1742ef3",
            Name = "CUSTOMER",
            ConcurrencyStamp = "02962efa-1273-46c0-b103-7167b1742ef3",
            NormalizedName = "customer"
        });
       builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.HOTEL,
            Id = Guid.Parse("34115F50-25D5-4750-8500-5CB917F27DA5"),
            RatingId = Enum.Rating.LODGING
        }); builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.HOTEL,
            Id = Guid.Parse("1E955290-E063-439F-8E17-D2EDCB2AD69B"),
            RatingId = Enum.Rating.HOTEL_TWOSTAR
        }); builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.HOTEL,
            Id = Guid.Parse("1BA853DB-84E4-4E9C-B750-DBE432D1A2B0"),
            RatingId = Enum.Rating.HOTEL_THREESTAR
        }); builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.HOTEL,
            Id = Guid.Parse("EF3FC5D9-7C7A-44C3-8FC1-8DA12CA7E93B"),
            RatingId = Enum.Rating.HOTEL_FOURSTAR
        }); builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.HOTEL,
            Id = Guid.Parse("F2603607-D947-4ACA-888E-9FD3BC3C0339"),
            RatingId = Enum.Rating.HOTEL_FIVESTAR
        });
        builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.RESTAURANTS,
            Id = Guid.Parse("F48D19EE-36AE-49BD-88ED-A3D326B1B1D8"),
            RatingId = Enum.Rating.RESTAURENT_CASUAL
        }); builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.RESTAURANTS,
            Id = Guid.Parse("23C8E757-AC2D-4232-A569-D5461DEB9B05"),
            RatingId = Enum.Rating.RESTAURENT_TWOSTAR
        }); builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.RESTAURANTS,
            Id = Guid.Parse("A59BBB50-4FCC-473F-BBF4-B311F06B2ED8"),
            RatingId = Enum.Rating.RESTAURENT_THREESTAR
        });
        builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.RESTAURANTS,
            Id = Guid.Parse("FA63BF87-51A3-46B7-A6DF-AA29FB9B9057"),
            RatingId = Enum.Rating.RESTAURENT_FOURSTAR
        });
        builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.RESTAURANTS,
            Id = Guid.Parse("4B3961FF-AA19-41FD-9A3E-1A6B37F99D09"),
            RatingId = Enum.Rating.RESTAURENT_FIVESTAR
        });
        builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.HOTEL,
            Id = Guid.Parse("D39D6346-6831-4155-B27A-A2A779271BA2"),
            RatingId = Enum.Rating.RESORT
        });
        builder.Entity<FacilityRating>().HasData(new FacilityRating
        {
            FacilityTypeId = Enum.FacilityType.ENTERTAINMENT,
            Id = Guid.Parse("994CB1A9-6D4E-4D34-B904-91B229A12D5C"),
            RatingId = Enum.Rating.TOURIST_AREA
        }); 

        base.OnModelCreating(builder);

        SeedEnumTable<Models.EnumModels.AttendanceRouteType, Domain.Enum.AttendanceRouteType>(builder);
        SeedEnumTable<Models.EnumModels.AttendanceType, Domain.Enum.AttendanceType>(builder);
        SeedEnumTable<Models.EnumModels.JoinTourStatus, Domain.Enum.JoinTourStatus>(builder);
        SeedEnumTable<Models.EnumModels.MaterialType, Domain.Enum.MaterialType>(builder);
        SeedEnumTable<Models.EnumModels.OptionClass, Domain.Enum.OptionClass>(builder);
        SeedEnumTable<Models.EnumModels.OptionQuotationStatus, Domain.Enum.OptionQuotationStatus>(builder);
        SeedEnumTable<Models.EnumModels.OrderStatus, Domain.Enum.OrderStatus>(builder);
        SeedEnumTable<Models.EnumModels.PrivateTourStatus, Domain.Enum.PrivateTourStatus>(builder);
        SeedEnumTable<Models.EnumModels.FacilityType, Domain.Enum.FacilityType>(builder);
        SeedEnumTable<Models.EnumModels.TourStatus, Domain.Enum.TourStatus>(builder);
        SeedEnumTable<Models.EnumModels.TourType, Domain.Enum.TourType>(builder);
        SeedEnumTable<Models.EnumModels.TransactionType, Domain.Enum.TransactionType>(builder);
        SeedEnumTable<Models.EnumModels.Unit, Domain.Enum.Unit>(builder);
        SeedEnumTable<Models.EnumModels.VehicleType, Domain.Enum.VehicleType>(builder);
        SeedEnumTable<Models.EnumModels.ServiceAvailability, Domain.Enum.ServiceAvailability>(builder);
        SeedEnumTable<Models.EnumModels.Rating, Domain.Enum.Rating>(builder);
        SeedEnumTable<Models.EnumModels.ServiceType, Domain.Enum.ServiceType>(builder);
        SeedEnumTable<Models.EnumModels.MealType, Domain.Enum.MealType>(builder);
        SeedEnumTable<Models.EnumModels.DietaryPreference, Domain.Enum.DietaryPreference>(builder);
        SeedEnumTable<Models.EnumModels.ReferencePriceRating, Domain.Enum.ReferencePriceRating>(builder);
        SeedEnumTable<Models.EnumModels.ManagementFeeType, Domain.Enum.ManagementFeeType>(builder);
    }

    private static void SeedEnumTable<TEntity, TEnum>(ModelBuilder modelBuilder)
             where TEntity : class
             where TEnum : System.Enum
    {
        var enumType = typeof(TEnum);
        var entityType = typeof(TEntity);

        if (!enumType.IsEnum)
        {
            throw new ArgumentException("TEnum must be an enum type.");
        }

        var enumValues = System.Enum.GetValues(enumType).Cast<TEnum>();

        foreach (var enumValue in enumValues)
        {
            var entityInstance = Activator.CreateInstance(entityType);
            entityType.GetProperty("Id")!.SetValue(entityInstance, enumValue);
            entityType.GetProperty("Name")!.SetValue(entityInstance, enumValue.ToString());
            modelBuilder.Entity<TEntity>().HasData(entityInstance!);
        }
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    IConfiguration config = new ConfigurationBuilder()
    //                   .SetBasePath(Directory.GetCurrentDirectory())
    //                   .AddJsonFile("appsettings.json", true, true)
    //                   .Build();
    //    string cs = config["ConnectionStrings:Host"];
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseSqlServer(cs);
    //    }
    //    optionsBuilder.UseSqlServer(
    //       "server=.;database=db_aa9541_travelcapstone2;uid=sa;pwd=12345;TrustServerCertificate=True;MultipleActiveResultSets=True;");
    //}
}