using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.ConfigurationModel;
using TravelCapstone.BackEnd.Common.DTO.Payment.PaymentLibrary;
using TravelCapstone.BackEnd.Common.DTO.Payment.PaymentRequest;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Models;
using Transaction = TravelCapstone.BackEnd.Domain.Models.Transaction;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class WalletService : GenericBackendService, IWalletService
    {
        private IRepository<Transaction> _transactionRepository;
        private MomoConfiguration _momoConfiguration;
        private VnPayConfiguration _vnPayConfiguration;
        private readonly IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;

        public WalletService(IConfiguration configuration, IRepository<Transaction> repository, IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _transactionRepository = repository;
            _configuration = configuration;
            _momoConfiguration = Resolve<MomoConfiguration>()!;
            _vnPayConfiguration = Resolve<VnPayConfiguration>()!;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetUrlVnPayRecharge(Guid travelcompanionId, double amount)
        {
            AppActionResult result = new AppActionResult();
            var travelCompanionRepository = Resolve<IRepository<TravelCompanion>>();
            try
            {
                var travelCompanion = await travelCompanionRepository!.GetById(travelcompanionId);
                if (travelCompanion == null)
                {
                    result = BuildAppActionResultError(result,
                        $"Travel companion với id {travelcompanionId} không tồn tại");
                }

                if (amount < 100000 || amount > 100000000)
                {
                    result = BuildAppActionResultError(result,
                        $"Hệ thống chỉ hỗ trợ nạp tối thiểu 10.000 đồng và tối đa là  100.000.000 đồng");
                }

                if (!BuildAppActionResultIsError(result))
                {
                    PaymentInformationRequest momo = new PaymentInformationRequest
                    {
                        AccountID = travelcompanionId.ToString(),
                        Amount = amount,
                        CustomerName = $"{travelCompanion!.FirstName} {travelCompanion.LastName}",
                    };
                    var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
                    var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
                    var pay = new VNPayLibrary();
                    var urlCallBack = $"{_vnPayConfiguration.ReturnUrl}";

                    pay.AddRequestData("vnp_Version", _vnPayConfiguration.Version!);
                    pay.AddRequestData("vnp_Command", _vnPayConfiguration.Command!);
                    pay.AddRequestData("vnp_TmnCode", _vnPayConfiguration.TmnCode!);
                    pay.AddRequestData("vnp_Amount", (amount * 100).ToString());
                    pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
                    pay.AddRequestData("vnp_CurrCode", _vnPayConfiguration.CurrCode!);
                    pay.AddRequestData("vnp_IpAddr", pay.GenerateRandomIPAddress());
                    pay.AddRequestData("vnp_Locale", _vnPayConfiguration.Locale!);
                    pay.AddRequestData("vnp_OrderInfo",
                        $"Khách hàng: {travelCompanion!.FirstName} {travelCompanion.LastName} nạp tiền vào hệ thống travel");
                    pay.AddRequestData("vnp_OrderType", "other");
                    pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
                    pay.AddRequestData("vnp_TxnRef", $"{travelcompanionId.ToString()} {Guid.NewGuid()}");

                    result.Result = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"],
                        _configuration["Vnpay:HashSecret"]);
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> GetUrlMomoRecharge(Guid travelcompanionId, double amount)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var travelCompanionRepository = Resolve<IRepository<TravelCompanion>>();
                var travelCompanion = await travelCompanionRepository!.GetById(travelcompanionId);
                if (travelCompanion == null)
                {
                    result = BuildAppActionResultError(result,
                        $"Travel companion với id {travelcompanionId} không tồn tại");
                }

                if (amount < 100000 || amount > 100000000)
                {
                    result = BuildAppActionResultError(result,
                        $"Hệ thống chỉ hỗ trợ nạp tối thiểu 10.000 đồng và tối đa là  100.000.000 đồng");
                }

                if (!BuildAppActionResultIsError(result))
                {
                    PaymentInformationRequest momo = new PaymentInformationRequest
                    {
                        AccountID = travelcompanionId.ToString(),
                        Amount = amount,
                        CustomerName = $"{travelCompanion!.FirstName} {travelCompanion.LastName}",
                    };

                    string endpoint = _momoConfiguration.Api!;
                    string partnerCode = _momoConfiguration.PartnerCode!;
                    string accessKey = _momoConfiguration.AccessKey!;
                    string secretkey = _momoConfiguration.Secretkey!;
                    string orderInfo =
                        $"Khách hàng: {travelCompanion!.FirstName} {travelCompanion.LastName} nạp tiền vào hệ thống travel";
                    string redirectUrl = $"{_momoConfiguration.RedirectUrl}";
                    string ipnUrl = _momoConfiguration.IPNUrl!;
                    //  string ipnUrl = "https://webhook.site/3399b42a-eee3-4e2d-8925-c2f893737de9";

                    string requestType = "captureWallet";

                    string amountString = Math.Ceiling(momo.Amount).ToString();
                    string orderId = Guid.NewGuid().ToString();
                    string requestId = Guid.NewGuid().ToString();
                    string extraData = travelcompanionId.ToString();

                    //Before sign HMAC SHA256 signature
                    string rawHash = "accessKey=" + accessKey +
                                     "&amount=" + amount +
                                     "&extraData=" + extraData +
                                     "&ipnUrl=" + ipnUrl +
                                     "&orderId=" + orderId +
                                     "&orderInfo=" + orderInfo +
                                     "&partnerCode=" + partnerCode +
                                     "&redirectUrl=" + redirectUrl +
                                     "&requestId=" + requestId +
                                     "&requestType=" + requestType
                        ;

                    MomoSecurity crypto = new MomoSecurity();
                    //sign signature SHA256
                    string signature = crypto.signSHA256(rawHash, secretkey);

                    //build body json request
                    JObject message = new JObject
                    {
                        { "partnerCode", partnerCode },
                        { "partnerName", "Test" },
                        { "storeId", "MomoTestStore" },
                        { "requestId", requestId },
                        { "amount", amount },
                        { "orderId", orderId },
                        { "orderInfo", orderInfo },
                        { "redirectUrl", redirectUrl },
                        { "ipnUrl", ipnUrl },
                        { "lang", "en" },
                        { "extraData", extraData },
                        { "requestType", requestType },
                        { "signature", signature }
                    };

                    var client = new RestClient();

                    var request = new RestRequest(endpoint, Method.Post);
                    request.AddJsonBody(message.ToString());
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        JObject jmessage = JObject.Parse(response.Content!);
                        result.Result = jmessage.GetValue("payUrl")!.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> Recharge(Guid travelcompanionId, double amount)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var travelCompanionRepository = Resolve<IRepository<TravelCompanion>>();
                var utility = Resolve<Utility>();
                var travelCompanion = await travelCompanionRepository!.GetById(travelcompanionId);
                if (travelCompanion == null)
                {
                    result = BuildAppActionResultError(result,
                        $"Travel companion với id {travelcompanionId} không tồn tại");
                }

                if (amount < 100000 || amount > 100000000)
                {
                    result = BuildAppActionResultError(result,
                        $"Hệ thống chỉ hỗ trợ nạp tối thiểu 10.000 đồng và tối đa là  100.000.000 đồng");
                }

                if (!BuildAppActionResultIsError(result))
                {
                    await _transactionRepository.Insert(
                        new Transaction
                        {
                            Amount = amount,
                            Date = utility!.GetCurrentDateTimeInTimeZone(),
                            Id = Guid.NewGuid(),
                            TravelCompanionId = travelcompanionId,
                        }
                    );
                    travelCompanion!.Money += amount;
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> Pay(Guid orderId)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var orderRepository = Resolve<IRepository<Order>>();
                var travelCompanionRepository = Resolve<IRepository<TravelCompanion>>();

                var utility = Resolve<Utility>();
                var order = await orderRepository!.GetById(orderId);
                if (order == null)
                {
                    result = BuildAppActionResultError(result, $"Biên lai với id {orderId} không tồn tại");
                }

                var travelCompanion = await travelCompanionRepository!.GetById(order!.TravelCompanionId);
                if (travelCompanion == null)
                {
                    result = BuildAppActionResultError(result,
                        $"Travel companion với id {order.TravelCompanionId} không tồn tại");
                }

                if (!BuildAppActionResultIsError(result))
                {
                    await _transactionRepository.Insert(
                        new Transaction
                        {
                            Amount = -order!.Total,
                            Date = utility!.GetCurrentDateTimeInTimeZone(),
                            Id = Guid.NewGuid(),
                            TravelCompanionId = order.TravelCompanionId,
                        }
                    );
                    travelCompanion!.Money += -order!.Total;

                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> GetTravelCompanion(Guid travelCompanionId)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var travelCompanionRepository = Resolve<IRepository<TravelCompanion>>();
                var travelCompanion = await travelCompanionRepository!.GetById(travelCompanionId);
                if (travelCompanion == null)
                {
                    result = BuildAppActionResultError(result,
                        $"Travel companion với id {travelCompanionId} không tồn tại");
                }

                if (!BuildAppActionResultIsError(result))
                {
                    result.Result = travelCompanion;
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> GetAllTransaction(Guid travelcompanionId, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var data = await _transactionRepository.GetAllDataByExpression(
                     a => a.TravelCompanionId == travelcompanionId,
                     pageNumber, pageSize
                 );
                data.Items = data.Items!.OrderByDescending(a => a.Date).ToList();
                result.Result = data;
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }
    }
}