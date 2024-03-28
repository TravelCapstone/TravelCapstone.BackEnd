using DinkToPdf;
using DinkToPdf.Contracts;
using Firebase.Storage;
using OfficeOpenXml;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Infrastructure.Mapping;

namespace TravelCapstone.BackEnd.API.Installers;

public class LibraryInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        var mapper = MappingConfig.RegisterMap().CreateMapper();
        services.AddSingleton(mapper);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSignalR();
        services.AddSingleton<BackEndLogger>();
        services.AddSingleton(_ => new FirebaseStorage(configuration["Firebase:Bucket"]));
        services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        services.AddSingleton<Utility>();
        services.AddSingleton<SD>();
        services.AddSingleton<TemplateMappingHelper>();
    }
}