﻿using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers;

[Route("account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(
        IAccountService accountService
    )
    {
        _accountService = accountService;
    }

    [HttpPost("create-account")]
    public async Task<AppActionResult> CreateAccount(SignUpRequestDto request)
    {
        return await _accountService.CreateAccount(request, false);
    }

    [HttpGet("get-all-account")]
    public async Task<AppActionResult> GetAllAccount(int pageIndex = 1, int pageSize = 10)
    {
        return await _accountService.GetAllAccount(pageIndex, pageSize);
    }

    [HttpPost("login")]
    public async Task<AppActionResult> Login(LoginRequestDto request)
    {
        return await _accountService.Login(request);
    }

    [HttpPut("update-account")]
    public async Task<AppActionResult> UpdateAccount(UpdateAccountRequestDto request)
    {
        return await _accountService.UpdateAccount(request);
    }

    [HttpPost("get-account-by-userId/{id}")]
    public async Task<AppActionResult> GetAccountByUserId(string id)
    {
        return await _accountService.GetAccountByUserId(id);
    }

    [HttpPut("change-password")]
    public async Task<AppActionResult> ChangePassword(ChangePasswordDto dto)
    {
        return await _accountService.ChangePassword(dto);
    }

    [HttpPost("get-new-token/{userId}")]
    public async Task<AppActionResult> GetNewToken([FromBody] string refreshToken, string userId)
    {
        return await _accountService.GetNewToken(refreshToken, userId);
    }

    [HttpPut("forgot-password")]
    public async Task<AppActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        return await _accountService.ForgotPassword(dto);
    }

    [HttpPut("active-account/{email}/{verifyCode}")]
    public async Task<AppActionResult> ActiveAccount(string email, string verifyCode)
    {
        return await _accountService.ActiveAccount(email, verifyCode);
    }

    [HttpPost("send-email-forgot-password/{email}")]
    public async Task<AppActionResult> SendEmailForgotPassword(string email)
    {
        return await _accountService.SendEmailForgotPassword(email);
    }

    [HttpPost("send-email-for-activeCode/{email}")]
    public async Task<AppActionResult> SendEmailForActiveCode(string email)
    {
        return await _accountService.SendEmailForActiveCode(email);
    }

    [HttpPost("google-callback")]
    public async Task<AppActionResult> GoogleCallBack([FromBody] string accessTokenFromGoogle)
    {
        return await _accountService.GoogleCallBack(accessTokenFromGoogle);
    }
}