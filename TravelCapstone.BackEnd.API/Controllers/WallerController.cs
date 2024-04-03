﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Payment.PaymentRespone;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("wallet")]
    [ApiController]
    public class WallerController : ControllerBase
    {
        private IWalletService _walletService;

        public WallerController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        [HttpGet("get-url-vnpay-recharge")]
        public async Task<AppActionResult> GetUrlVnPayRecharge(Guid travelCompanionId, double amount)
        {
            return await _walletService.GetUrlVnPayRecharge(travelCompanionId, amount);
        }
        [HttpGet("get-url-momo-recharge")]
        public async Task<AppActionResult> GetUrlMomoRecharge(Guid travelCompanionId, double amount)
        {
            return await _walletService.GetUrlMomoRecharge(travelCompanionId, amount);
        }
        [HttpPost("pay/{orderId}")]
        public async Task<AppActionResult> Pay(Guid orderId)
        {
            return await _walletService.Pay(orderId);
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
                    await _walletService.Recharge(Guid.Parse(response.OrderId), double.Parse(response.Amount));
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("MomoIpn")]
        public async Task<IActionResult> MomoIPN(MomoResponseDto momo)
        {
            try
            {
                if (momo.resultCode == 0)
                {
                    await _walletService.Recharge(Guid.Parse(momo.extraData), double.Parse(momo.amount.ToString()));
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
