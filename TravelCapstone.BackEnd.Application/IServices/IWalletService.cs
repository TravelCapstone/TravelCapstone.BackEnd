using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IWalletService
    {
        Task<AppActionResult> GetUrlVnPayRecharge(Guid travelcompanionId, double amount);
        Task<AppActionResult> GetUrlMomoRecharge(Guid travelcompanionId, double amount);
        Task<AppActionResult> GetAllTransaction(Guid travelcompanionId, int pageNumber, int pageSize);
        Task<AppActionResult> Recharge(Guid travelcompanionId, double amount);
        Task<AppActionResult> Pay(Guid orderId);
        Task<AppActionResult> GetTravelCompanion(Guid travelCompanionId);

    }
}
