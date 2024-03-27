using Hangfire.Dashboard;

namespace TravelCapstone.BackEnd.API.Middlewares;

public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}