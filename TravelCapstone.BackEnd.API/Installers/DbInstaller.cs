using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelCapstone.BackEnd.Domain.Data;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.API.Installers;

public class DbInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TravelCapstoneDbContext>(option =>
        {
            option.UseSqlServer(configuration["ConnectionStrings:DB"]);
        });

        services.AddIdentity<Account, IdentityRole>().AddEntityFrameworkStores<TravelCapstoneDbContext>()
            .AddDefaultTokenProviders();
    }
}