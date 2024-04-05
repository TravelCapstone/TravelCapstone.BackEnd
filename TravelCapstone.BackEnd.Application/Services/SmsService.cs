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

        public async Task<AppActionResult> SendMessage(string message)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                string accountSid = "AC38b11d7ee74f1a987f6ad678556ea98d";
                string authToken = "50f849812d522983fcf64a99c9210421";
                TwilioClient.Init(accountSid, authToken);

                var apiResponsse = MessageResource.Create(
                   body: $"{message}",
                   from: new Twilio.Types.PhoneNumber("+13106516534"),
                   to: new Twilio.Types.PhoneNumber("+84366967957") //add receiver's phone number
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