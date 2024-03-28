using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO;

namespace TravelCapstone.BackEnd.API.Controllers;

[Route("api/[controller]")]
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

    [HttpPost(nameof(CreateAccount))]
    public async Task<AppActionResult> CreateAccount(SignUpRequestDto request)
    {
        return await _accountService.CreateAccount(request, false);
    }

    [HttpGet(nameof(GetAllAccount))]
    public async Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize)
    {
        return await _accountService.GetAllAccount(pageIndex, pageSize);
    }

    [HttpPost(nameof(Login))]
    public async Task<AppActionResult> Login(LoginRequestDto request)
    {
        return await _accountService.Login(request);
    }

    [HttpPut(nameof(UpdateAccount))]
    public async Task<AppActionResult> UpdateAccount(UpdateAccountRequestDto request)
    {
        return await _accountService.UpdateAccount(request);
    }

    [HttpPost("GetAccountByUserId/{id}")]
    public async Task<AppActionResult> GetAccountByUserId(string id)
    {
        return await _accountService.GetAccountByUserId(id);
    }


    [HttpPut(nameof(ChangePassword))]
    public async Task<AppActionResult> ChangePassword(ChangePasswordDto dto)
    {
        return await _accountService.ChangePassword(dto);
    }


    [HttpPost("GetNewToken/{userId}")]
    public async Task<AppActionResult> GetNewToken([FromBody] string refreshToken, string userId)
    {
        return await _accountService.GetNewToken(refreshToken, userId);
    }

    [HttpPut(nameof(ForgotPassword))]
    public async Task<AppActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        return await _accountService.ForgotPassword(dto);
    }

    [HttpPut("ActiveAccount/{email}/{verifyCode}")]
    public async Task<AppActionResult> ActiveAccount(string email, string verifyCode)
    {
        return await _accountService.ActiveAccount(email, verifyCode);
    }

    [HttpPost("SendEmailForgotPassword/{email}")]
    public async Task<AppActionResult> SendEmailForgotPassword(string email)
    {
        return await _accountService.SendEmailForgotPassword(email);
    }

    [HttpPost("SendEmailForActiveCode/{email}")]
    public async Task<AppActionResult> SendEmailForActiveCode(string email)
    {
        return await _accountService.SendEmailForActiveCode(email);
    }

    [HttpPost(nameof(GoogleCallBack))]
    public async Task<AppActionResult> GoogleCallBack([FromBody] string accessTokenFromGoogle)
    {
        return await _accountService.GoogleCallBack(accessTokenFromGoogle);
    }
}