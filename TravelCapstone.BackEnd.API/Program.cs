using Microsoft.EntityFrameworkCore;
using TravelCapstone.BackEnd.API.Installers;
using TravelCapstone.BackEnd.Domain.Data;
using TravelCapstone.BackEnd.Infrastructure.ServerHub;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(p => p.AddPolicy(MyAllowSpecificOrigins, builder =>
{
    builder.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:5175",
            "https://hcqs-backend.azurewebsites.net/", "https://love-house.vercel.app")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials(); // Add this line to allow credentials

    // Other configurations...
}));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.InstallerServicesInAssembly(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger(op => op.SerializeAsV2 = true);
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
    options.RoutePrefix = string.Empty;
});
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>(nameof(NotificationHub));
//ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<TravelCapstoneDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0) _db.Database.Migrate();
    }
}