using NetCore.QK.DbContext;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Application.Services;
using TravelCapstone.BackEnd.Domain.Data;
using TravelCapstone.BackEnd.Infrastructure.Repositories;

namespace TravelCapstone.BackEnd.API.Installers;

public class ServiceInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbContext, TravelCapstoneDbContext>();
        services.AddScoped<IAccountRepository,AccountRepository>();
        services.AddScoped<IUserRoleRepository,UserRoleRepository>();
        services.AddScoped<IIdentityRoleRepository,IdentityRoleRepository>();

        services.AddScoped<IJwtService,JwtService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAccountService,AccountService>();
        services.AddScoped<IEmailService, EmailService>();
    }
}