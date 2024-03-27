using Microsoft.AspNetCore.SignalR;
using TravelCapstone.BackEnd.Application.IHubServices;
using TravelCapstone.BackEnd.Infrastructure.ServerHub;

namespace TravelCapstone.BackEnd.Infrastructure.HubServices;

public class HubServices : IHubServices
{
    private readonly IHubContext<NotificationHub> _signalRHub;

    public HubServices(IHubContext<NotificationHub> signalRHub)
    {
        _signalRHub = signalRHub;
    }

    public async Task SendAsync(string method)
    {
        await _signalRHub.Clients.All.SendAsync(method);
    }
}