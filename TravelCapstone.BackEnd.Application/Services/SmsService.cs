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
                string accountSid = "AC4671e2f0ecc6bb9ede82c7c3538a58af";
                string authToken = "3ccb7de6877a8bd77952baffcda4eb43";
                TwilioClient.Init(accountSid, authToken);

                var apiResponsse = MessageResource.Create(
                   body: $"{message}",
                   from: new Twilio.Types.PhoneNumber("+17208623332"),
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