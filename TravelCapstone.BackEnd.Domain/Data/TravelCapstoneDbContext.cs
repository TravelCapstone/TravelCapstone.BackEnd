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

    public override DbSet<T> Set<T>() where T : class
    {
        return base.Set<T>();
    }

    public override EntityEntry<T> Entry<T>(T entity) where T : class
    {
        return base.Entry(entity);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

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