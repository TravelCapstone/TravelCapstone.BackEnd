using AutoMapper;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.ConfigurationModel;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Models;
using Utility = TravelCapstone.BackEnd.Common.Utils.Utility;
using Firebase.Auth;
using StackExchange.Redis;
using NPOI.SS.Formula.Functions;

namespace TravelCapstone.BackEnd.Application.Services;

public class AccountService : GenericBackendService, IAccountService
{
    private readonly IRepository<Account> _accountRepository;
    private readonly IMapper _mapper;
    private readonly SignInManager<Account> _signInManager;
    private readonly TokenDto _tokenDto;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Account> _userManager;
    private readonly IEmailService _emailService;
    private readonly IExcelService _excelService;
    private readonly IFileService _fileService;
    public AccountService(
        IRepository<Account> accountRepository,
        IUnitOfWork unitOfWork,
        UserManager<Account> userManager,
        SignInManager<Account> signInManager,
        IEmailService emailService,
        IExcelService excelService,
        IFileService fileService,
        IMapper mapper,
        IServiceProvider serviceProvider
    ) : base(serviceProvider)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _excelService = excelService;
        _tokenDto = new TokenDto();
        _mapper = mapper;
    }

    public async Task<AppActionResult> Login(LoginRequestDto loginRequest)
    {
        var result = new AppActionResult();
        try
        {
            var user = await _accountRepository.GetByExpression(u =>
                u!.Email.ToLower() == loginRequest.Email.ToLower() && u.IsDeleted == false);
            if (user == null)
                result = BuildAppActionResultError(result, $" {loginRequest.Email} này không tồn tại trong hệ thống");
            else if (user.IsVerified == false)
                result = BuildAppActionResultError(result, "Tài khoản này chưa được xác thực !");

            var passwordSignIn =
                await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);
            if (!passwordSignIn.Succeeded) result = BuildAppActionResultError(result, "Đăng nhâp thất bại");
            if (!BuildAppActionResultIsError(result)) result = await LoginDefault(loginRequest.Email, user);
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> VerifyLoginGoogle(string email, string verifyCode)
    {
        var result = new AppActionResult();
        try
        {
            var user = await _accountRepository.GetByExpression(u =>
                u!.Email.ToLower() == email.ToLower() && u.IsDeleted == false);
            if (user == null)
                result = BuildAppActionResultError(result, $"Email này không tồn tại");
            else if (user.IsVerified == false)
                result = BuildAppActionResultError(result, "Tài khoản này chưa xác thực !");
            else if (user.VerifyCode != verifyCode)
                result = BuildAppActionResultError(result, "Mã xác thực sai!");

            if (!BuildAppActionResultIsError(result))
            {
                result = await LoginDefault(email, user);
                user!.VerifyCode = null;
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> CreateAccount(SignUpRequestDto signUpRequest, bool isGoogle)
    {
        var result = new AppActionResult();
        try
        {
            if (await _accountRepository.GetByExpression(r => r!.UserName == signUpRequest.Email) != null)
                result = BuildAppActionResultError(result, "Email hoặc username không tồn tại!");

            if (!BuildAppActionResultIsError(result))
            {
                var emailService = Resolve<IEmailService>();
                var verifyCode = string.Empty;
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
                    result.Result = user;
                    if (!isGoogle)
                        emailService!.SendEmail(user.Email, SD.SubjectMail.VERIFY_ACCOUNT,
                            TemplateMappingHelper.GetTemplateOTPEmail(
                                TemplateMappingHelper.ContentEmailType.VERIFICATION_CODE, verifyCode,
                                user.FirstName));
                }
                else
                {
                    result = BuildAppActionResultError(result, $"Tạo tài khoản không thành công");
                }

                var resultCreateRole = await _userManager.AddToRoleAsync(user, "CUSTOMER");
                if (!resultCreateRole.Succeeded) result = BuildAppActionResultError(result, $"Cấp quyền khách hàng không thành công");
            }
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> UpdateAccount(UpdateAccountRequestDto accountRequest)
    {
        var result = new AppActionResult();
        try
        {
            var account =
                await _accountRepository.GetByExpression(
                    a => a!.UserName.ToLower() == accountRequest.Email.ToLower());
            if (account == null)
                result = BuildAppActionResultError(result, $"Tài khoản với email {accountRequest.Email} không tồn tại!");
            if (!BuildAppActionResultIsError(result))
            {
                account!.FirstName = accountRequest.FirstName;
                account.LastName = accountRequest.LastName;
                account.PhoneNumber = accountRequest.PhoneNumber;
                result.Result = await _accountRepository.Update(account);
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> GetAccountByUserId(string id)
    {
        var result = new AppActionResult();
        try
        {
            var account = await _accountRepository.GetById(id);
            if (account == null) result = BuildAppActionResultError(result, $"Tài khoản với id {id} không tồn tại !");
            if (!BuildAppActionResultIsError(result)) result.Result = account;
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize)
    {
        var result = new AppActionResult();
        var list = await _accountRepository.GetAllDataByExpression(null, pageIndex, pageSize, null,false,null);

        var userRoleRepository = Resolve<IRepository<IdentityUserRole<string>>>();
        var roleRepository = Resolve<IRepository<IdentityRole>>();
        var listRole = await roleRepository!.GetAllDataByExpression(null, 1, 100, null, false, null);
        var listMap = _mapper.Map<List<AccountResponse>>(list.Items);
        foreach (var item in listMap)
        {
            var userRole = new List<IdentityRole>();
            var role = await userRoleRepository!.GetAllDataByExpression(a => a.UserId == item.Id, 1, 100, null, false, null);
            foreach (var itemRole in role.Items!)
            {
                var roleUser = listRole.Items!.ToList().FirstOrDefault(a => a.Id == itemRole.RoleId);
                if (roleUser != null) userRole.Add(roleUser);
            }

            item.Role = userRole;
        }

        result.Result =
            new PagedResult<AccountResponse>
            { Items = listMap, TotalPages = list.TotalPages };
        return result;
    }

    public async Task<AppActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var result = new AppActionResult();

        try
        {
            if (await _accountRepository.GetByExpression(c =>
                    c!.Email == changePasswordDto.Email && c.IsDeleted == false) == null)
                result = BuildAppActionResultError(result,
                    $"Tài khoản có email {changePasswordDto.Email} không tồn tại!");
            if (!BuildAppActionResultIsError(result))
            {
                var user = await _accountRepository.GetByExpression(c =>
                    c!.Email == changePasswordDto.Email && c.IsDeleted == false);
                var changePassword = await _userManager.ChangePasswordAsync(user!, changePasswordDto.OldPassword,
                    changePasswordDto.NewPassword);
                if (!changePassword.Succeeded)
                    result = BuildAppActionResultError(result, "Thay đổi mật khẩu thất bại");
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> GetNewToken(string refreshToken, string userId)
    {
        var result = new AppActionResult();

        try
        {
            var user = await _accountRepository.GetById(userId);
            if (user == null)
                result = BuildAppActionResultError(result, "Tài khoản không tồn tại");
            else if (user.RefreshToken != refreshToken)
                result = BuildAppActionResultError(result, "Mã làm mới không chính xác");

            if (!BuildAppActionResultIsError(result))
            {
                var jwtService = Resolve<IJwtService>();
                result.Result = await jwtService!.GetNewToken(refreshToken, userId);
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        var result = new AppActionResult();

        try
        {
            var user = await _accountRepository.GetByExpression(a =>
                a!.Email == dto.Email && a.IsDeleted == false && a.IsVerified == true);
            if (user == null)
                result = BuildAppActionResultError(result, "Tài khoản không tồn tại hoặc chưa được xác thực!");
            else if (user.VerifyCode != dto.RecoveryCode)
                result = BuildAppActionResultError(result, "Mã xác thực sai!");

            if (!BuildAppActionResultIsError(result))
            {
                await _userManager.RemovePasswordAsync(user!);
                var addPassword = await _userManager.AddPasswordAsync(user!, dto.NewPassword);
                if (addPassword.Succeeded)
                    user!.VerifyCode = null;
                else
                    result = BuildAppActionResultError(result, "Thay đổi mật khẩu thất bại. Vui lòng thử lại");
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> ActiveAccount(string email, string verifyCode)
    {
        var result = new AppActionResult();
        try
        {
            var user = await _accountRepository.GetByExpression(a =>
                a!.Email == email && a.IsDeleted == false && a.IsVerified == false);
            if (user == null)
                result = BuildAppActionResultError(result, "Tài khoản không tồn tại ");
            else if (user.VerifyCode != verifyCode)
                result = BuildAppActionResultError(result, "Mã xác thực sai");

            if (!BuildAppActionResultIsError(result))
            {
                user!.IsVerified = true;
                user.VerifyCode = null;
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> SendEmailForgotPassword(string email)
    {
        var result = new AppActionResult();

        try
        {
            var user = await _accountRepository.GetByExpression(a =>
                a!.Email == email && a.IsDeleted == false && a.IsVerified == true);
            if (user == null) result = BuildAppActionResultError(result, "Tài khoản không tồn tại hoặc chưa được xác thực");

            if (!BuildAppActionResultIsError(result))
            {
                var emailService = Resolve<IEmailService>();
                var code = await GenerateVerifyCode(user!.Email, true);
                emailService?.SendEmail(email, SD.SubjectMail.PASSCODE_FORGOT_PASSWORD,
                    TemplateMappingHelper.GetTemplateOTPEmail(TemplateMappingHelper.ContentEmailType.FORGOTPASSWORD,
                        code, user.FirstName!));
            }
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> SendEmailForActiveCode(string email)
    {
        var result = new AppActionResult();

        try
        {
            var user = await _accountRepository.GetByExpression(a =>
                a!.Email == email && a.IsDeleted == false && a.IsVerified == false);
            if (user == null) result = BuildAppActionResultError(result, "Tài khoản không tồn tại hoặc chưa xác thực");

            if (!BuildAppActionResultIsError(result))
            {
                var emailService = Resolve<IEmailService>();
                var code = await GenerateVerifyCode(user!.Email, false);
                emailService!.SendEmail(email, SD.SubjectMail.VERIFY_ACCOUNT,
                    TemplateMappingHelper.GetTemplateOTPEmail(TemplateMappingHelper.ContentEmailType.VERIFICATION_CODE,
                        code, user.FirstName!));
            }
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<string> GenerateVerifyCode(string email, bool isForForgettingPassword)
    {
        var code = string.Empty;

        var user = await _accountRepository.GetByExpression(a =>
            a!.Email == email && a.IsDeleted == false && a.IsVerified == isForForgettingPassword);

        if (user != null)
        {
            code = Guid.NewGuid().ToString("N").Substring(0, 6);
            user.VerifyCode = code;
        }

        await _unitOfWork.SaveChangesAsync();

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
                    type = config!.Type,
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

            var verifiedToken = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance
                .VerifyIdTokenAsync(accessTokenFromGoogle);
            var emailClaim = verifiedToken.Claims.FirstOrDefault(c => c.Key == "email");
            var nameClaim = verifiedToken.Claims.FirstOrDefault(c => c.Key == "name");
            var name = nameClaim.Value.ToString();
            var userEmail = emailClaim.Value.ToString();

            if (userEmail != null)
            {
                var user = await _accountRepository.GetByExpression(a => a!.Email == userEmail && a.IsDeleted == false);
                if (user == null)
                {
                    var resultCreate =
                        await CreateAccount(
                            new SignUpRequestDto
                            {
                                Email = userEmail,
                                FirstName = name!,
                                Gender = true,
                                LastName = string.Empty,
                                Password = "Google123@",
                                PhoneNumber = string.Empty
                            }, true);
                    if (resultCreate.IsSuccess)
                    {
                        var account = (Account)resultCreate.Result!;
                        result = await LoginDefault(userEmail, account);
                    }
                }

                result = await LoginDefault(userEmail, user);
            }
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    private async Task<AppActionResult> LoginDefault(string email, Account? user)
    {
        var result = new AppActionResult();

        var jwtService = Resolve<IJwtService>();
        var utility = Resolve<Utility>();
        var token = await jwtService!.GenerateAccessToken(new LoginRequestDto { Email = email });

        if (user!.RefreshToken == null)
        {
            user.RefreshToken = jwtService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = utility!.GetCurrentDateInTimeZone().AddDays(1);
        }

        if (user.RefreshTokenExpiryTime <= utility!.GetCurrentDateInTimeZone())
        {
            user.RefreshTokenExpiryTime = utility.GetCurrentDateInTimeZone().AddDays(1);
            user.RefreshToken = jwtService.GenerateRefreshToken();
        }

        _tokenDto.Token = token;
        _tokenDto.RefreshToken = user.RefreshToken;
        result.Result = _tokenDto;
        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<AppActionResult> AssignRoleForUserId(string userId, IList<string> roleId)
    {
        var result = new AppActionResult();
        try
        {
            var user = await _accountRepository.GetById(userId);
            var userRoleRepository = Resolve<IRepository<IdentityUserRole<string>>>();
            var identityRoleRepository = Resolve<IRepository<IdentityRole>>();
            foreach (var role in roleId)
                if (await identityRoleRepository!.GetById(role) == null)
                    result = BuildAppActionResultError(result, $"Vai trò với id {role} không tồn tại");

            if (!BuildAppActionResultIsError(result))
                foreach (var role in roleId)
                {
                    var roleDb = await identityRoleRepository!.GetById(role);
                    var resultCreateRole = await _userManager.AddToRoleAsync(user, roleDb.NormalizedName);
                    if (!resultCreateRole.Succeeded)
                        result = BuildAppActionResultError(result, $"Cấp quyền với vai trò {role} không thành công");
                }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> RemoveRoleForUserId(string userId, IList<string> roleId)
    {
        var result = new AppActionResult();

        try
        {
            var user = await _accountRepository.GetById(userId);
            var userRoleRepository = Resolve<IRepository<IdentityUserRole<string>>>();
            var identityRoleRepository = Resolve<IRepository<IdentityRole>>();
            if (user == null)
                result = BuildAppActionResultError(result, $"Người dùng với {userId} không tồn tại");
            foreach (var role in roleId)
                if (await identityRoleRepository.GetById(role) == null)
                    result = BuildAppActionResultError(result, $"Vai trò với {role} không tồn tại");

            if (!BuildAppActionResultIsError(result))
                foreach (var role in roleId)
                {
                    var roleDb = await identityRoleRepository!.GetById(role);
                    var resultCreateRole = await _userManager.RemoveFromRoleAsync(user!, roleDb.NormalizedName);
                    if (!resultCreateRole.Succeeded)
                        result = BuildAppActionResultError(result, $"Xóa quyền {role} thất bại");
                }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }


    public async Task<AppActionResult> ImportTourGuideFromExcel(IFormFile file)
    {
        AppActionResult result = new AppActionResult();
        try
        {
            var validation = await ValidateExcelFile(file);
            ExcelValidatingResponse validationResponse = validation.Result as ExcelValidatingResponse;
            if (validationResponse == null)
            {
                result = BuildAppActionResultError(result, "Kiểm tra file Excel xảy ra lỗi.\n Vui lòng thử lại.");
                return result;
            }
            if (!validationResponse.IsValidated)
            {
                result.Result = validationResponse;
                return result;
            }

            List<TourGuideRegistrationRecord> records = await GetListFromExcel(file);
            List<Account> tourGuideList = new List<Account>();
            foreach (TourGuideRegistrationRecord record in records)
            {
                var tourGuideAccount = _mapper.Map<Account>(record);
                tourGuideAccount.Id = Guid.NewGuid().ToString();
                tourGuideAccount.UserName = record.Email;
                tourGuideAccount.IsDeleted = false;
                tourGuideAccount.IsVerified = true;
                tourGuideAccount.VerifyCode = null;
                var resultCreateUser = await _userManager.CreateAsync(tourGuideAccount, SD.DEFAULT_PASSWORD);
                if (!resultCreateUser.Succeeded)
                {
                    result.Messages.Add($"Tạo tài khoản cho hướng dẫn viên {record.FirstName} {record.LastName} với số điện thoại {record.PhoneNumber} thất bại.");
                }
                else
                {
                    tourGuideList.Add(tourGuideAccount);
                }
            }
            bool isSuccessful = await AssignTourGuideRole(tourGuideList);
            if (isSuccessful)
            {
                SendAccountCreationEmailForTourGuide(tourGuideList);
            }

        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }
        return result;
    }

    public async Task<AppActionResult> ValidateExcelFile(IFormFile file)
    {
        AppActionResult result = new AppActionResult();
        ExcelValidatingResponse data = new ExcelValidatingResponse();
        try
        {
            string errorHeader = await _excelService.CheckHeader(file, SD.ExcelHeaders.TOURGUIDE_REGISTRATION);
            if (!string.IsNullOrEmpty(errorHeader))
            {
                data.IsValidated = false;
                data.HeaderError = errorHeader;
                result.Result = data;
                return result;
            }

            List<TourGuideRegistrationRecord> records = await GetListFromExcel(file);
            if (records.Count == 0)
            {
                data.IsValidated = false;
                data.HeaderError = $"Danh sách báo giá rỗng";
                result.Result = data;
                return result;
            }

            int errorRecordCount = 0;
            int i = 2;
            int invalidRowInput = 0;
            string key = "";
            data.Errors = new string[records.Count];
            Dictionary<string, int> duplicatedData = new Dictionary<string, int>();
            string[] words = null;
            PagedResult<Account> accountDb = null;
            foreach (TourGuideRegistrationRecord record in records)
            {
                StringBuilder error = new StringBuilder();
                errorRecordCount = 0;
                Guid serviceId = Guid.Empty;
                if (record.No != i - 1)
                {
                    error.Append($"{errorRecordCount + 1}. Thứ tự đúng {i - 1}.\n");
                    errorRecordCount++;
                }

                if (string.IsNullOrEmpty(record.FirstName) || string.IsNullOrEmpty(record.LastName) || string.IsNullOrEmpty(record.Email) || string.IsNullOrEmpty(record.PhoneNumber))
                {
                    error.Append($"{errorRecordCount + 1}. Ô tên hoặc đơn vị tính trống.\n");
                    errorRecordCount++;
                    continue;
                }

                //SD.EnumType.serviceUnit.TryGetValue(record.Unit, out int serviceUnit);
                //Check Regex

                if (!Regex.IsMatch(record.FirstName, SD.Regex.NAME))
                {
                    error.Append($"{errorRecordCount + 1}. Tên không đúng format\n");
                    errorRecordCount++;
                }

                words = record.FirstName.Split(' ');
                if (words.Length > 1)
                {
                    foreach (string word in words)
                    {
                        if (char.GetUnicodeCategory(word[0]) == UnicodeCategory.UppercaseLetter)
                        {
                            error.Append($"{errorRecordCount + 1}. Mỗi chữ cái đầu trong tên phải viết hoa.\n");
                            errorRecordCount++;
                            break;
                        }
                    }
                }

                words = record.LastName.Split(' ');
                if (words.Length > 1)
                {
                    foreach (string word in words)
                    {
                        if (char.GetUnicodeCategory(word[0]) == UnicodeCategory.UppercaseLetter)
                        {
                            error.Append($"{errorRecordCount + 1}. Mỗi chữ cái đầu trong tên phải viết hoa.\n");
                            errorRecordCount++;
                            break;
                        }
                    }
                }

                if (!Regex.IsMatch(record.LastName, SD.Regex.NAME))
                {
                    error.Append($"{errorRecordCount + 1}. Tên không đúng format\n");
                    errorRecordCount++;
                }

                if (!Regex.IsMatch(record.Email, SD.Regex.EMAIL))
                {
                    error.Append($"{errorRecordCount + 1}. Email không đúng format\n");
                    errorRecordCount++;
                }

                if (!Regex.IsMatch(record.PhoneNumber, SD.Regex.PHONENUMBER))
                {
                    error.Append($"{errorRecordCount + 1}. Số điện thoại không đúng format\n");
                    errorRecordCount++;
                }

                if (duplicatedData.ContainsKey(record.PhoneNumber))
                {
                    error.Append($"{errorRecordCount + 1}. Tồn tại số điện thoại này ở dòng {duplicatedData[record.PhoneNumber]}\n");
                    errorRecordCount++;
                }
                else
                {
                    duplicatedData.Add(record.PhoneNumber, i - 1);
                }

                if (duplicatedData.ContainsKey(record.Email))
                {
                    error.Append($"{errorRecordCount + 1}. Tồn tại email này ở dòng {duplicatedData[record.Email]}\n");
                    errorRecordCount++;
                }
                else
                {
                    duplicatedData.Add(record.Email, i - 1);
                }


                accountDb = await _accountRepository!.GetAllDataByExpression(a => a.NormalizedEmail == record.Email || a.PhoneNumber == record.PhoneNumber, 0, 0,null,false,null);
                if (accountDb.Items!.Count > 0)
                {
                    if (accountDb.Items.Where(a => a.NormalizedEmail == record.Email).Count() > 0)
                    {
                        error.Append($"{errorRecordCount + 1}. Email đã tồn tại trong hộ thống.\n");
                        errorRecordCount++;
                    }

                    if (accountDb.Items.Where(a => a.PhoneNumber == record.PhoneNumber).Count() > 0)
                    {
                        error.Append($"{errorRecordCount + 1}. Số điện thoại đã tồn tại trong hộ thống.\n");
                        errorRecordCount++;
                    }
                }

                if (errorRecordCount != 0)
                {
                    data.Errors[i - 2] = error.ToString();
                    invalidRowInput++;
                }
                i++;
            }

            if (invalidRowInput > 0)
            {
                data.IsValidated = false;
                result.Result = data;
                return result;
            }

            data.IsValidated = true;
            data.Errors = null;
            data.HeaderError = null;
            result.Result = data;
        }
        catch (Exception ex)
        {
            data.IsValidated = false;
        }
        return result;

    }

    private async Task<List<TourGuideRegistrationRecord>> GetListFromExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        try
        {
            using (MemoryStream stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    List<TourGuideRegistrationRecord> records = new List<TourGuideRegistrationRecord>();

                    for (int row = 2; row <= rowCount; row++) // Assuming header is in the first row
                    {
                        TourGuideRegistrationRecord record = new TourGuideRegistrationRecord()
                        {
                            No = (worksheet.Cells[row, 1].Value == null) ? 0 : int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                            FirstName = (worksheet.Cells[row, 2].Value == null) ? "" : worksheet.Cells[row, 2].Value.ToString(),
                            LastName = (worksheet.Cells[row, 3].Value == null) ? "" : worksheet.Cells[row, 3].Value.ToString(),
                            Email = (worksheet.Cells[row, 4].Value == null) ? "" : (worksheet.Cells[row, 4].Value.ToString()),
                            PhoneNumber = (worksheet.Cells[row, 5].Value == null) ? "" : (worksheet.Cells[row, 5].Value.ToString()),
                            Gender = (worksheet.Cells[row, 6].Value == null) ? false : (worksheet.Cells[row, 6].Value.ToString() == "TRUE")
                        };
                        records.Add(record);
                    }
                    return records;
                }
            }
        }
        catch (Exception ex)
        {
        }
        return null;
    }

    public void SendAccountCreationEmailForTourGuide(List<Account> tourGuideAccountList)
    {
        try
        {
            foreach (var account in tourGuideAccountList)
            {
                _emailService?.SendEmail(account.Email, $"Thông tin tài khoản của hướng dẫn viên {account.FirstName} {account.LastName} tại Cóc Travel",
                   TemplateMappingHelper.GetTemplateOTPEmail(TemplateMappingHelper.ContentEmailType.TOURGUIDE_ACCOUNT_CREATION,
                       $"Tên đăng nhập: {account.Email} \nMật khẩu: {SD.DEFAULT_PASSWORD}\n Vui lòng không chia sẻ thông tin tài khoản của bạn với bất kì ai", $"{account.FirstName} {account.LastName}"));
            }
        }
        catch (Exception ex)
        {
        }
    }

    public async Task<bool> AssignTourGuideRole(List<Account> tourGuideAccountList)
    {
        bool isSucess = true;
        try
        {
            var identityRoleRepository = Resolve<IRepository<IdentityRole>>();
            var roleDb = await identityRoleRepository!.GetByExpression(r => r.Name.Equals("TOUR GUIDE"));
            foreach (var account in tourGuideAccountList)
            {
                var accountDb = await _accountRepository.GetByExpression(a => a.PhoneNumber == account.PhoneNumber);
                if (accountDb != null)
                {
                    await AssignRoleForUserId(accountDb.Id, new List<string> { roleDb.Id });
                }
                else
                {
                    isSucess = false;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
        }
        return isSucess;
    }

    public async Task<IActionResult> GetTourGuideTemplate()
    {
        IActionResult result = null;
        try
        {
            List<TourGuideRegistrationRecord> sampleData = new List<TourGuideRegistrationRecord>();
            sampleData.Add(new TourGuideRegistrationRecord
            { No = 1, FirstName = "Anh", LastName = "Nguyễn", Email = "anhnguyen.tourguide@gmail.com", PhoneNumber = "0945787123", Gender = true });
            //result = _fileService.GenerateExcelContent<TourGuideRegistrationRecord>(sampleData, SD.ExcelHeaders.TOURGUIDE_REGISTRATION, "IMPORT TOURGUIDE");
        }
        catch (Exception ex)
        {
        }
        return result;
    }

    public async Task<AppActionResult> GetAccountsByRoleName(string roleName, int pageNumber, int pageSize)
    {
        var result = new AppActionResult();

        try
        {
            var roleRepository = Resolve<IRepository<IdentityRole>>();
            var roleDb = await roleRepository!.GetByExpression(r => r.NormalizedName.Equals(roleName.ToLower()));
            if(roleDb != null)
            {
                var userRoleRepository = Resolve<IRepository<IdentityUserRole<string>>>();
                var userRoleDb = await userRoleRepository!.GetAllDataByExpression(u => u.RoleId == roleDb.Id, 0, 0, null, false, null);
                if(userRoleDb.Items != null && userRoleDb.Items.Count > 0) 
                {
                    var accountIds = userRoleDb.Items.Select(u => u.UserId).Distinct().ToList();
                    var accountDb = await _accountRepository.GetAllDataByExpression(a => accountIds.Contains(a.Id), pageNumber, pageSize, null, false, null);
                    result.Result = accountDb;
                }
            }
            else
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy vai trò {roleName}");
            }
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> GetAccountsByRoleId(Guid Id, int pageNumber, int pageSize)
    {
        var result = new AppActionResult();

        try
        {
            var roleRepository = Resolve<IRepository<IdentityRole>>();
            var roleDb = await roleRepository!.GetById(Id);
            if (roleDb != null)
            {
                var userRoleRepository = Resolve<IRepository<IdentityUserRole<string>>>();
                var userRoleDb = await userRoleRepository!.GetAllDataByExpression(u => u.RoleId == roleDb.Id, 0, 0, null, false, null);
                if (userRoleDb.Items != null && userRoleDb.Items.Count > 0)
                {
                    var accountIds = userRoleDb.Items.Select(u => u.UserId).Distinct().ToList();
                    var accountDb = await _accountRepository.GetAllDataByExpression(a => accountIds.Contains(a.Id), pageNumber, pageSize, null, false, null);
                    result.Result = accountDb;
                }
            }
            else
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy vai trò với id {Id}");
            }
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }
}
