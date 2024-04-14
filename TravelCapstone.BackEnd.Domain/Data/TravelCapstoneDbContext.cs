using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<TourTraveller> TourTravellers { get; set; }
    public DbSet<AttendanceDetail> AttendanceDetails { get; set; }
    public DbSet<AttendanceRoute> AttendanceRoutes { get; set; }
    public DbSet<Commune> Communes { get; set; }
    public DbSet<DayPlan> DayPlans { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Material> Materials { get; set; }
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
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceCostHistory> ServiceCostHistorys { get; set; }
    public DbSet<ServiceProvider> ServiceProviders { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourRegistration> TourRegistrations { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleRoute> VehicleRoutes { get; set; }
    public DbSet<ServiceRating> ServiceRatings { get; set; }    
    public DbSet<Models.EnumModels.AttendanceRouteType> AttendanceRouteTypes { get; set; }
    public DbSet<Models.EnumModels.AttendanceType> AttendanceTypes { get; set; }
    public DbSet<Models.EnumModels.JoinTourStatus> JoinTourStatuses { get; set; }
    public DbSet<Models.EnumModels.MainVehicleTour> MainVehicleTours { get; set; }
    public DbSet<Models.EnumModels.MaterialType> MaterialTypes { get; set; }
    public DbSet<Models.EnumModels.OptionClass> OptionClasses { get; set; }
    public DbSet<Models.EnumModels.OptionQuotationStatus> OptionQuotationStatuses { get; set; }
    public DbSet<Models.EnumModels.OrderStatus> OrderStatuses { get; set; }
    public DbSet<Models.EnumModels.PrivateTourStatus> PrivateTourStatuses { get; set; }
    public DbSet<Models.EnumModels.ServiceType> ServiceTypes { get; set; }
    public DbSet<Models.EnumModels.TourStatus> TourStatuses { get; set; }
    public DbSet<Models.EnumModels.TourType> TourTypes { get; set; }
    public DbSet<Models.EnumModels.TransactionType> TransactionTypes { get; set; }
    public DbSet<Models.EnumModels.Unit> Units { get; set; }
    public DbSet<Models.EnumModels.VehicleType> VehicleTypes { get; set; }

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
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "server=.;database=TravelCapstone;uid=sa;pwd=12345;TrustServerCertificate=True;MultipleActiveResultSets=True;");
    }
}