using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;

namespace RealEstateManagement.Business.Services.Mail
{
    public class TwilioSmsService : ISmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _twilioPhoneNumber;

        public TwilioSmsService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _twilioPhoneNumber = configuration["Twilio:PhoneNumber"];
            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task SendOtpAsync(string phoneNumber, string otp)
        {
            var message = await MessageResource.CreateAsync(
                from: new Twilio.Types.PhoneNumber(_twilioPhoneNumber),
                to: new Twilio.Types.PhoneNumber(phoneNumber),
                body: $"Your OTP is: {otp}. Valid for 5 minutes."
            );
        }
    }

}