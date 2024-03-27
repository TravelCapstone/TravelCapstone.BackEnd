using System.Transactions;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using NetCore.QK.DbContext;
using Newtonsoft.Json;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.ConfigurationModel;
using TravelCapstone.BackEnd.Common.DTO;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Models;
using Utility = TravelCapstone.BackEnd.Common.Utils.Utility;

namespace TravelCapstone.BackEnd.Application.Services;

public class AccountService : GenericBackendService, IAccountService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<Account> _signInManager;
    private readonly TokenDto _tokenDto;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Account> _userManager;
    private readonly IAccountRepository _accountRepository;
    private readonly BackEndLogger _logger;

    public AccountService(
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork,
        UserManager<Account> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<Account> signInManager,
        IServiceProvider serviceProvider,
        BackEndLogger logger
    ) : base(serviceProvider)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _tokenDto = new TokenDto();
        _logger = logger;
    }

    public async Task<AppActionResult> Login(LoginRequestDto loginRequest)
    {
        var result = new AppActionResult();
        try
        {
            var user = await _accountRepository.GetByExpression(u =>
                u.Email.ToLower() == loginRequest.Email.ToLower() && u.IsDeleted == false);
            if (user == null)
                result = BuildAppActionResultError(result, $"The user with username {loginRequest.Email} not found");
            else if (user.IsVerified == false)
                result = BuildAppActionResultError(result, "The account is not verified !");

            var PasswordSignIn =
                await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);
            if (!PasswordSignIn.Succeeded) result = BuildAppActionResultError(result, SD.ResponseMessage.LOGIN_FAILED);

            if (!BuildAppActionResultIsError(result)) result = await LoginDefault(loginRequest.Email, user);
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
            _logger.LogError(ex.Message, this);
        }

        return result;
    }

    public async Task<AppActionResult> VerifyLoginGoogle(string email, string verifyCode)
    {
        var result = new AppActionResult();
        try
        {
            var user = await _accountRepository.GetByExpression(u =>
                u.Email.ToLower() == email.ToLower() && u.IsDeleted == false);
            if (user == null)
                result = BuildAppActionResultError(result, $"The user with username {email} not found");
            else if (user.IsVerified == false)
                result = BuildAppActionResultError(result, "The account is not verified !");
            else if (user.VerifyCode != verifyCode)
                result = BuildAppActionResultError(result, "The  verify code is wrong !");

            if (!BuildAppActionResultIsError(result))
            {
                result = await LoginDefault(email, user);
                user.VerifyCode = null;
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
            _logger.LogError(ex.Message, this);
        }

        return result;
    }

    public async Task<AppActionResult> CreateAccount(SignUpRequestDto signUpRequest, bool isGoogle)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var result = new AppActionResult();
            try
            {
                var userRoleRepository = Resolve<IUserRoleRepository>();
                var identityRoleRepository = Resolve<IIdentityRoleRepository>();
                if (await _accountRepository.GetByExpression(r => r.UserName == signUpRequest.Email) != null)
                    result = BuildAppActionResultError(result, "The email or username is existed");

                if (!BuildAppActionResultIsError(result))
                {
                    var emailService = Resolve<IEmailService>();
                    string verifyCode = null;
                    if (!isGoogle) verifyCode = Guid.NewGuid().ToString("N").Substring(0, 6);

                    var user = new Account
                    {
                        Email = signUpRequest.Email,
                        UserName = signUpRequest.Email,
                        FirstName = signUpRequest.FirstName,
                        LastName = signUpRequest.LastName,
                        PhoneNumber = signUpRequest.PhoneNumber,
                        Gender = signUpRequest.Gender,
                        VerifyCode = verifyCode,
                        IsVerified = isGoogle ? true : false
                    };
                    var resultCreateUser = await _userManager.CreateAsync(user, signUpRequest.Password);
                    if (resultCreateUser.Succeeded)
                    {
                        result.Result.Data = user;
                        if (!isGoogle)
                            emailService.SendEmail(user.Email, SD.SubjectMail.VERIFY_ACCOUNT,
                                TemplateMappingHelper.GetTemplateOTPEmail(
                                    TemplateMappingHelper.ContentEmailType.VERIFICATION_CODE, verifyCode,
                                    user.FirstName));
                    }
                    else
                    {
                        result = BuildAppActionResultError(result, $"{SD.ResponseMessage.CREATE_FAILED} USER");
                    }

                    var resultCreateRole = await _userManager.AddToRoleAsync(user, "CUSTOMER");
                    if (!resultCreateRole.Succeeded) result = BuildAppActionResultError(result, "ASSIGN ROLE FAILED");
                }

                if (!BuildAppActionResultIsError(result)) scope.Complete();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
                _logger.LogError(ex.Message, this);
            }

            return result;
        }
    }

    public async Task<AppActionResult> UpdateAccount(UpdateAccountRequestDto accountRequest)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var result = new AppActionResult();
            try
            {
                var account =
                    await _accountRepository.GetByExpression(
                        a => a.UserName.ToLower() == accountRequest.Email.ToLower());
                if (account == null)
                    result = BuildAppActionResultError(result, $"The user with email {account.Email} not found");
                if (!BuildAppActionResultIsError(result))
                {
                    account.FirstName = accountRequest.FirstName;
                    account.LastName = accountRequest.LastName;
                    account.PhoneNumber = accountRequest.PhoneNumber;
                    result.Result.Data = await _accountRepository.Update(account);
                }

                await _unitOfWork.SaveChangesAsync();
                if (!BuildAppActionResultIsError(result)) scope.Complete();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
                _logger.LogError(ex.Message, this);
            }

            return result;
        }
    }

    public async Task<AppActionResult> GetAccountByUserId(string id)
    {
        var result = new AppActionResult();
        try
        {
            var account = await _accountRepository.GetById(id);
            if (account == null) result = BuildAppActionResultError(result, $"The user with id {id} not found");
            if (!BuildAppActionResultIsError(result)) result.Result.Data = account;
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize)
    {
        throw new NotImplementedException();
    }


    public async Task<AppActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var result = new AppActionResult();

            try
            {
                if (await _accountRepository.GetByExpression(c =>
                        c.Email == changePasswordDto.Email && c.IsDeleted == false) == null)
                    result = BuildAppActionResultError(result,
                        $"The user with email {changePasswordDto.Email} not found");
                if (!BuildAppActionResultIsError(result))
                {
                    var user = await _accountRepository.GetByExpression(c =>
                        c.Email == changePasswordDto.Email && c.IsDeleted == false);
                    var ChangePassword = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword,
                        changePasswordDto.NewPassword);
                    if (!ChangePassword.Succeeded)
                        result = BuildAppActionResultError(result, SD.ResponseMessage.CREATE_FAILED);
                }

                await _unitOfWork.SaveChangesAsync();
                if (!BuildAppActionResultIsError(result)) scope.Complete();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
                _logger.LogError(ex.Message, this);
            }

            return result;
        }
    }

    public async Task<AppActionResult> GetNewToken(string refreshToken, string userId)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var result = new AppActionResult();

            try
            {
                var user = await _accountRepository.GetById(userId);
                if (user == null)
                    result = BuildAppActionResultError(result, "The user is not existed");
                else if (user.RefreshToken != refreshToken)
                    result = BuildAppActionResultError(result, "The refresh token is not exacted");

                if (!BuildAppActionResultIsError(result))
                {
                    var jwtService = Resolve<IJwtService>();
                    result.Result.Data = await jwtService.GetNewToken(refreshToken, userId);
                }

                await _unitOfWork.SaveChangesAsync();
                if (!BuildAppActionResultIsError(result)) scope.Complete();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
                _logger.LogError(ex.Message, this);
            }

            return result;
        }
    }

    public async Task<AppActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var result = new AppActionResult();

            try
            {
                var user = await _accountRepository.GetByExpression(a =>
                    a.Email == dto.Email && a.IsDeleted == false && a.IsVerified == true);
                if (user == null)
                    result = BuildAppActionResultError(result, "The user is not existed or is not verified");
                else if (user.VerifyCode != dto.RecoveryCode)
                    result = BuildAppActionResultError(result, "The verification code is wrong.");

                if (!BuildAppActionResultIsError(result))
                {
                    await _userManager.RemovePasswordAsync(user);
                    var AddPassword = await _userManager.AddPasswordAsync(user, dto.NewPassword);
                    if (AddPassword.Succeeded)
                        user.VerifyCode = null;
                    else
                        result = BuildAppActionResultError(result, "Change password failed");
                }

                await _unitOfWork.SaveChangesAsync();
                if (!BuildAppActionResultIsError(result)) scope.Complete();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
                _logger.LogError(ex.Message, this);
            }

            return result;
        }
    }

    public async Task<AppActionResult> ActiveAccount(string email, string verifyCode)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var result = new AppActionResult();
            try
            {
                var user = await _accountRepository.GetByExpression(a =>
                    a.Email == email && a.IsDeleted == false && a.IsVerified == false);
                if (user == null)
                    result = BuildAppActionResultError(result, "The user is not existed ");
                else if (user.VerifyCode != verifyCode)
                    result = BuildAppActionResultError(result, "The verification code is wrong.");

                if (!BuildAppActionResultIsError(result))
                {
                    user.IsVerified = true;
                    user.VerifyCode = null;
                }

                await _unitOfWork.SaveChangesAsync();
                if (!BuildAppActionResultIsError(result)) scope.Complete();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
                _logger.LogError(ex.Message, this);
            }

            return result;
        }
    }

    public async Task<AppActionResult> SendEmailForgotPassword(string email)
    {
        var result = new AppActionResult();

        try
        {
            var user = await _accountRepository.GetByExpression(a =>
                a.Email == email && a.IsDeleted == false && a.IsVerified == true);
            if (user == null) result = BuildAppActionResultError(result, "The user is not existed or is not verified");

            if (!BuildAppActionResultIsError(result))
            {
                var emailService = Resolve<IEmailService>();
                var code = await GenerateVerifyCode(user.Email, true);
                emailService.SendEmail(email, SD.SubjectMail.PASSCODE_FORGOT_PASSWORD,
                    TemplateMappingHelper.GetTemplateOTPEmail(TemplateMappingHelper.ContentEmailType.FORGOTPASSWORD,
                        code, user.FirstName));
            }
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
            _logger.LogError(ex.Message, this);
        }

        return result;
    }

    public async Task<AppActionResult> SendEmailForActiveCode(string email)
    {
        var result = new AppActionResult();

        try
        {
            var user = await _accountRepository.GetByExpression(a =>
                a.Email == email && a.IsDeleted == false && a.IsVerified == false);
            if (user == null) result = BuildAppActionResultError(result, "The user does not existed or is verified");

            if (!BuildAppActionResultIsError(result))
            {
                var emailService = Resolve<IEmailService>();
                var code = await GenerateVerifyCode(user.Email, false);
                emailService.SendEmail(email, SD.SubjectMail.VERIFY_ACCOUNT,
                    TemplateMappingHelper.GetTemplateOTPEmail(TemplateMappingHelper.ContentEmailType.VERIFICATION_CODE,
                        code, user.FirstName));
            }
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
            _logger.LogError(ex.Message, this);
        }

        return result;
    }

    public async Task<string> GenerateVerifyCode(string email, bool isForForgettingPassword)
    {
        var code = string.Empty;
        try
        {
            var user = await _accountRepository.GetByExpression(a =>
                a.Email == email && a.IsDeleted == false && a.IsVerified == isForForgettingPassword);

            if (user != null)
            {
                code = Guid.NewGuid().ToString("N").Substring(0, 6);
                user.VerifyCode = code;
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, this);
        }

        return code;
    }

    public async Task<AppActionResult> GoogleCallBack(string accessTokenFromGoogle)
    {
        var result = new AppActionResult();
        try
        {
            var existingFirebaseApp = FirebaseApp.DefaultInstance;
            if (existingFirebaseApp == null)
            {
                var config = Resolve<FirebaseAdminSDK>();
                var credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(new
                {
                    type = config.Type,
                    project_id = config.Project_id,
                    private_key_id = config.Private_key_id,
                    private_key = config.Private_key,
                    client_email = config.Client_email,
                    client_id = config.Client_id,
                    auth_uri = config.Auth_uri,
                    token_uri = config.Token_uri,
                    auth_provider_x509_cert_url = config.Auth_provider_x509_cert_url,
                    client_x509_cert_url = config.Client_x509_cert_url
                }));
                var firebaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = credential
                });
            }

            var verifiedToken = await FirebaseAuth.DefaultInstance
                .VerifyIdTokenAsync(accessTokenFromGoogle);
            var emailClaim = verifiedToken.Claims.FirstOrDefault(c => c.Key == "email");
            var nameClaim = verifiedToken.Claims.FirstOrDefault(c => c.Key == "name");
            var name = nameClaim.Value.ToString();
            var userEmail = emailClaim.Value.ToString();

            if (userEmail != null)
            {
                var user = await _accountRepository.GetByExpression(a => a.Email == userEmail && a.IsDeleted == false);
                if (user == null)
                {
                    var resultCreate =
                        await CreateAccount(
                            new SignUpRequestDto
                            {
                                Email = userEmail, FirstName = name, Gender = true, LastName = string.Empty,
                                Password = "Google123@", PhoneNumber = string.Empty
                            }, true);
                    if (resultCreate != null && resultCreate.IsSuccess)
                    {
                        var account = (Account)resultCreate.Result.Data;
                        result = await LoginDefault(userEmail, account);
                    }
                }

                result = await LoginDefault(userEmail, user);
            }
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
            _logger.LogError(ex.Message, this);
        }

        return result;
    }

    private async Task<AppActionResult> LoginDefault(string email, Account? user)
    {
        var result = new AppActionResult();

        try
        {
            var jwtService = Resolve<IJwtService>();
            var utility = Resolve<Utility>();
            var token = await jwtService.GenerateAccessToken(new LoginRequestDto { Email = email });

            if (user.RefreshToken == null)
            {
                user.RefreshToken = jwtService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = utility.GetCurrentDateInTimeZone().AddDays(1);
            }

            if (user.RefreshTokenExpiryTime <= utility.GetCurrentDateInTimeZone())
            {
                user.RefreshTokenExpiryTime = utility.GetCurrentDateInTimeZone().AddDays(1);
                user.RefreshToken = jwtService.GenerateRefreshToken();
            }

            _tokenDto.Token = token;
            _tokenDto.RefreshToken = user.RefreshToken;
            result.Result.Data = _tokenDto;
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, this);
        }

        return result;
    }


    public async Task<AppActionResult> AssignRoleForUserId(string userId, IList<string> roleId)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var result = new AppActionResult();
            try
            {
                var user = await _accountRepository.GetById(userId);
                var userRoleRepository = Resolve<IUserRoleRepository>();
                var identityRoleRepository = Resolve<IIdentityRoleRepository>();
                if (user == null)
                    result = BuildAppActionResultError(result, $"The user with id {userId} is not existed");
                foreach (var role in roleId)
                    if (await identityRoleRepository.GetById(role) == null)
                        result = BuildAppActionResultError(result, $"The role with id {role} is not existed");

                if (!BuildAppActionResultIsError(result))
                    foreach (var role in roleId)
                    {
                        var roleDB = await identityRoleRepository.GetById(role);
                        var resultCreateRole = await _userManager.AddToRoleAsync(user, roleDB.NormalizedName);
                        if (!resultCreateRole.Succeeded)
                            result = BuildAppActionResultError(result, $"ASSIGN ROLE {role}  FAILED");
                    }

                await _unitOfWork.SaveChangesAsync();
                if (!BuildAppActionResultIsError(result)) scope.Complete();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
                _logger.LogError(ex.Message, this);
            }

            return result;
        }
    }

    public async Task<AppActionResult> RemoveRoleForUserId(string userId, IList<string> roleId)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var result = new AppActionResult();

            try
            {
                var user = await _accountRepository.GetById(userId);
                var userRoleRepository = Resolve<IUserRoleRepository>();
                var identityRoleRepository = Resolve<IIdentityRoleRepository>();
                if (user == null)
                    result = BuildAppActionResultError(result, $"The user with id {userId} is not existed");
                foreach (var role in roleId)
                    if (await identityRoleRepository.GetById(role) == null)
                        result = BuildAppActionResultError(result, $"The role with id {role} is not existed");

                if (!BuildAppActionResultIsError(result))
                    foreach (var role in roleId)
                    {
                        var roleDB = await identityRoleRepository.GetById(role);
                        var resultCreateRole = await _userManager.RemoveFromRoleAsync(user, roleDB.NormalizedName);
                        if (!resultCreateRole.Succeeded)
                            result = BuildAppActionResultError(result, $"REMOVE ROLE {role}  FAILED");
                    }

                await _unitOfWork.SaveChangesAsync();
                if (!BuildAppActionResultIsError(result)) scope.Complete();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
                _logger.LogError(ex.Message, this);
            }

            return result;
        }
    }
}