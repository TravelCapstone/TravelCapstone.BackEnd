using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<PlanServiceCostDetail> PlanServiceCostDetails { get; set; }
    public DbSet<PrivateJoinTourRequest> PrivateJoinTourRequests { get; set; }
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
    public DbSet<TravelCompanion> TravelCompanions { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleRoute> VehicleRoutes { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var cs = config["ConnectionStrings:DB"];
        cs =
            "server=.;database=TravelCapstone;uid=sa;pwd=12345;TrustServerCertificate=True;MultipleActiveResultSets=True;";
        if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer(cs);
    }
}