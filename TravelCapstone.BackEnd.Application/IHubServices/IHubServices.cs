namespace TravelCapstone.BackEnd.Application.IHubServices;

public interface IHubServices
{
    Task SendAsync(string method);
}