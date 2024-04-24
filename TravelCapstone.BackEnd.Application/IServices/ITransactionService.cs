using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface ITransactionService
    {
        Task<AppActionResult> GetUrlVnPayRecharge(Guid OrderId);

        Task<AppActionResult> GetUrlMomoRecharge(Guid OrderId);

        Task<AppActionResult> GetAllTransaction(Guid travelcompanionId, int pageNumber, int pageSize);

        Task<AppActionResult> Recharge(Guid travelcompanionId, double amount, TransactionType transactionType);

        Task<AppActionResult> Pay(Guid travelcompanionId, double amount);
        Task<AppActionResult> GetTravelCompanion(Guid travelCompanionId);
        Task<AppActionResult> UpdatesSucessStatus(Guid orderId);
    }
}