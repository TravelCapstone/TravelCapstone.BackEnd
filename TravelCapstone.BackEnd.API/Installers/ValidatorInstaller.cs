using FluentValidation;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.Validator;

namespace TravelCapstone.BackEnd.API.Installers;

public class ValidatorInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<HandleErrorValidator>();
        services.AddValidatorsFromAssemblyContaining<PrivateTourRequestDTO>();
    }
}