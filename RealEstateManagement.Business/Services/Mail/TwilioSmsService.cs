using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

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
            
            if (string.IsNullOrEmpty(_accountSid) || string.IsNullOrEmpty(_authToken) || string.IsNullOrEmpty(_twilioPhoneNumber))
            {
                throw new InvalidOperationException("Twilio configuration is missing. Please check appsettings.json");
            }
            
            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task SendOtpAsync(string phoneNumber, string otp)
        {
            try
            {
                var message = await MessageResource.CreateAsync(
                    from: new PhoneNumber(_twilioPhoneNumber),
                    to: new PhoneNumber(phoneNumber),
                    body: $"Mã OTP của bạn là: {otp}. Mã có hiệu lực trong 5 phút."
                );

                if (message.Status == MessageResource.StatusEnum.Failed)
                {
                    throw new Exception($"Failed to send SMS: {message.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send OTP via Twilio: {ex.Message}", ex);
            }
        }

        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                var smsMessage = await MessageResource.CreateAsync(
                    from: new PhoneNumber(_twilioPhoneNumber),
                    to: new PhoneNumber(phoneNumber),
                    body: message
                );

                if (smsMessage.Status == MessageResource.StatusEnum.Failed)
                {
                    throw new Exception($"Failed to send SMS: {smsMessage.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send SMS via Twilio: {ex.Message}", ex);
            }
        }

        public async Task SendPropertyInquiryAsync(string phoneNumber, string propertyTitle, string contactPhone)
        {
            var message = $"Bạn có tin nhắn mới về bất động sản: {propertyTitle}. Liên hệ: {contactPhone}";
            await SendSmsAsync(phoneNumber, message);
        }

        public async Task SendNotificationAsync(string phoneNumber, string notification)
        {
            await SendSmsAsync(phoneNumber, notification);
        }

        public async Task SendZaloMessageAsync(string phoneNumber, string message)
        {
            // Fallback to SMS if Zalo is not available
            await SendSmsAsync(phoneNumber, message);
        }
    }
} 