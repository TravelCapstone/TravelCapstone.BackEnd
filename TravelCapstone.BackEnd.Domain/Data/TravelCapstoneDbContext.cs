using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Domain.Data;

public class TravelCapstoneDbContext : IdentityDbContext<Account>
{
    public TravelCapstoneDbContext()
    {
    }

    public TravelCapstoneDbContext(DbContextOptions options) : base(options)
    {
    }
}