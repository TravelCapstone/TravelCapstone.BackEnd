using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class SmsService : GenericBackendService, ISmsService
    {
        public SmsService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<AppActionResult> SendMessage(string message, string phoneNumber)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                string accountSid = "AC16c01656bd500a1aa1ea9fe9040360d7";
                string authToken = "788dab3dcd68e060682311d3141b7299";
                TwilioClient.Init(accountSid, authToken);

                var apiResponsse = MessageResource.Create(
                   body: $"{message}",
                   from: new Twilio.Types.PhoneNumber("+1 443 333 1958"),
                   to: new Twilio.Types.PhoneNumber("+84366967957") 
               );

                result.Result = apiResponsse.Status;
            }
            catch (Exception e)
            {
                result = BuildAppActionResultError(result, e.Message);
            }

            return result;
        }
    }
}