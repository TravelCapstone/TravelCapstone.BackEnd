using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TravelCapstone.BackEnd.Domain.IData;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Domain.Data;

public class TravelCapstoneDbContext : IdentityDbContext<Account>, IDBContext
{
    public TravelCapstoneDbContext()
    {
    }

    public TravelCapstoneDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; } = null!;
    
    override
    public DbSet<T> Set<T>() where T : class
    {
        return base.Set<T>();
    }
    override
    public EntityEntry<T> Entry<T>(T entity) where T : class
    {
        return base.Entry(entity);
    }
}