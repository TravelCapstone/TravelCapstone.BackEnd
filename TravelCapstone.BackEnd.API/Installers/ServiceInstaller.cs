using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IUnitOfWork;
using TravelCapstone.BackEnd.Domain.Data;
using TravelCapstone.BackEnd.Domain.IData;
using TravelCapstone.BackEnd.Infrastructure.Repositories;
using TravelCapstone.BackEnd.Infrastructure.UnitOfWork;

namespace TravelCapstone.BackEnd.API.Installers;

public class ServiceInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbContext, TravelCapstoneDbContext>();
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}