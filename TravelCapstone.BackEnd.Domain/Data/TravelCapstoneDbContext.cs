using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Configuration> Configurations { get; set; } = null!;
    public DbSet<TourTraveller> TourTravellers { get; set; } = null!;
    public DbSet<AttendanceDetail> AttendanceDetails { get; set; } = null!;
    public DbSet<AttendanceRoute> AttendanceRoutes { get; set; } = null!;
    public DbSet<Communce> Communces { get; set; } = null!;
    public DbSet<DayPlan> DayPlans { get; set; } = null!;
    public DbSet<Destination> Destinations { get; set; } = null!;
    public DbSet<District> Districts { get; set; } = null!;
    public DbSet<Driver> Drivers { get; set; } = null!;
    public DbSet<Material> Materials { get; set; } = null!;
    public DbSet<OptionQuotation> OptionQuotations { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public DbSet<PlanServiceCostDetail> PlanServiceCostDetails { get; set; } = null!;
    public DbSet<PrivateJoinTourRequest> PrivateJoinTourRequests { get; set; } = null!;
    public DbSet<PrivateTourRequest> PrivateTourRequests { get; set; } = null!;
    public DbSet<Province> Provinces { get; set; } = null!;
    public DbSet<QuotationDetail> QuotationDetails { get; set; } = null!;
    public DbSet<Route> Routes { get; set; } = null!;
    public DbSet<SellPriceHistory> SellPriceHistorys { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<ServiceCostHistory> ServiceCostHistorys { get; set; } = null!;
    public DbSet<ServiceProvider> ServiceProviders { get; set; } = null!;
    public DbSet<Tour> Tours { get; set; } = null!;
    public DbSet<TourRegistration> TourRegistrations { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<TravelCompanion> TravelCompanions { get; set; } = null!; 
    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<VehicleRoute> VehicleRoutes { get; set; } = null!;



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var cs = config["ConnectionStrings:DB"];
        if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer(cs);
    }
}