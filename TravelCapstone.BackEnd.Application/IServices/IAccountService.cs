using TravelCapstone.BackEnd.Common.DTO;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface IAccountService
{
    Task<AppActionResult> Login(LoginRequestDto loginRequest);

    public Task<AppActionResult> VerifyLoginGoogle(string email, string verifyCode);

    Task<AppActionResult> CreateAccount(SignUpRequestDto signUpRequest, bool isGoogle);

    Task<AppActionResult> UpdateAccount(UpdateAccountRequestDto applicationUser);

    Task<AppActionResult> ChangePassword(ChangePasswordDto changePasswordDto);

    Task<AppActionResult> GetAccountByUserId(string id);

    Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize);


    Task<AppActionResult> GetNewToken(string refreshToken, string userId);

    Task<AppActionResult> ForgotPassword(ForgotPasswordDto dto);

    Task<AppActionResult> ActiveAccount(string email, string verifyCode);

    Task<AppActionResult> SendEmailForgotPassword(string email);

    Task<string> GenerateVerifyCode(string email, bool isForForgettingPassword);

    Task<AppActionResult> GoogleCallBack(string accessTokenFromGoogle);

    public Task<AppActionResult> SendEmailForActiveCode(string email);
}