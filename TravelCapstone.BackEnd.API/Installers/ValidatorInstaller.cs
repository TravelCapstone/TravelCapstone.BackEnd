using FluentValidation;
using HCQS.BackEnd.Common.Validator;
using TravelCapstone.BackEnd.Common.DTO;
namespace TravelCapstone.BackEnd.API.Installers;

public class ValidatorInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<HandleErrorValidator>();
        services.AddValidatorsFromAssemblyContaining<PrivateTourRequestDTO>();
    }
}