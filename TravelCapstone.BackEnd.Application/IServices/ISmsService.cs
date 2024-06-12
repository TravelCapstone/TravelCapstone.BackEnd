using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface ISmsService
    {
        Task<AppActionResult> SendMessage(string message, string phoneNumber);
    }
}