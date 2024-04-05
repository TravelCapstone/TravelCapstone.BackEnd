using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Payment.PaymentRespone;
using TravelCapstone.BackEnd.Common.DTO.Response;
using Twilio.Http;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("get-travel-companion")]
        public async Task<AppActionResult> GetTravelCompanion(Guid travelCompanionId)
        {
            return await _walletService.GetTravelCompanion(travelCompanionId);
        }

        [HttpGet("get-url-vnpay-recharge")]
        public async Task<AppActionResult> GetUrlVnPayRecharge(Guid orderId)
        {
            return await _walletService.GetUrlVnPayRecharge(orderId);
        }

        [HttpGet("get-url-momo-recharge")]
        public async Task<AppActionResult> GetUrlMomoRecharge(Guid orderId)
        {
            return await _walletService.GetUrlMomoRecharge(orderId);
        }

        [HttpGet("get-all-transaction")]
        public async Task<AppActionResult> GetAllTransaction(Guid travelCompanionId, int pageNumber = 1, int pageSize = 10)
        {
            return await _walletService.GetAllTransaction(travelCompanionId, pageNumber, pageSize);
        }


        [HttpGet("VNPayIpn")]
        public async Task<IActionResult> VNPayIPN()
        {
            try
            {
                var response = new VNPayResponseDto
                {
                    PaymentMethod = Request.Query["vnp_BankCode"],
                    OrderDescription = Request.Query["vnp_OrderInfo"],
                    OrderId = Request.Query["vnp_TxnRef"],
                    PaymentId = Request.Query["vnp_TransactionNo"],
                    TransactionId = Request.Query["vnp_TransactionNo"],
                    Token = Request.Query["vnp_SecureHash"],
                    VnPayResponseCode = Request.Query["vnp_ResponseCode"],
                    PayDate = Request.Query["vnp_PayDate"],
                    Amount = Request.Query["vnp_Amount"],
                    Success = true
                };

                if (response.VnPayResponseCode == "00")
                {
                    var orderId = response.OrderId.ToString().Split(" ");
                    await _walletService.UpdatesSucessStatus(Guid.Parse(orderId[0]));
                    await _walletService.Recharge(Guid.Parse(orderId[1]),double.Parse( response.Amount)/100, Domain.Enum.TransactionType.RECHARGE_FROM_VNPAY);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(new
            {
                RspCode = "00",
                Message = "Confirm Success"
            });
        }

        [HttpPost("MomoIpn")]
        public async Task<IActionResult> MomoIPN(MomoResponseDto momo)
        {
            try
            {
                if (momo.resultCode == 0)
                {
                    var orderId = momo.extraData.Split(" ");
                    await _walletService.UpdatesSucessStatus(Guid.Parse(orderId[0]));
                    await _walletService.Recharge(Guid.Parse(orderId[1]), double.Parse(momo.amount.ToString()) / 100, Domain.Enum.TransactionType.RECHARGE_FROM_VNPAY);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}