using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace RealEstateManagement.Business.Services.Mail
{
    public class TwilioSmsService : ISmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _verificationServiceSid;

        public TwilioSmsService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _verificationServiceSid = configuration["Twilio:VerificationServiceSid"];
            
            if (string.IsNullOrEmpty(_accountSid) || string.IsNullOrEmpty(_authToken) || string.IsNullOrEmpty(_verificationServiceSid))
            {
                throw new InvalidOperationException("Twilio configuration is missing. Please check appsettings.json");
            }
            
            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task SendOtpAsync(string phoneNumber, string otp)
        {
            try
            {
                // Using Twilio Verify API - Twilio will generate and send OTP automatically
                var verification = await VerificationResource.CreateAsync(
                    to: phoneNumber,
                    channel: "sms",
                    pathServiceSid: _verificationServiceSid
                );

                // Note: With Verify API, Twilio generates and sends the OTP automatically
                // We don't need to specify the OTP code ourselves
                // The verification.Sid can be used to check the verification status later
                
                // Store the verification SID for later verification
                // You might want to store this in your database or session
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send OTP via Twilio Verify: {ex.Message}", ex);
            }
        }

        // New method to verify OTP using Verify API
        public async Task<bool> VerifyOtpAsync(string phoneNumber, string otp)
        {
            try
            {
                var verificationCheck = await VerificationCheckResource.CreateAsync(
                    to: phoneNumber,
                    code: otp,
                    pathServiceSid: _verificationServiceSid
                );

                return verificationCheck.Status == "approved";
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to verify OTP via Twilio Verify: {ex.Message}", ex);
            }
        }

        // For regular SMS, we'll need a different approach
        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            // This method won't work with Verify API
            // You'll need to use a regular Twilio phone number for non-OTP SMS
            throw new NotImplementedException("SendSmsAsync is not supported with Verify API. Use SendOtpAsync instead.");
        }

        public async Task SendPropertyInquiryAsync(string phoneNumber, string propertyTitle, string contactPhone)
        {
            // This method won't work with Verify API
            throw new NotImplementedException("SendPropertyInquiryAsync is not supported with Verify API.");
        }

        public async Task SendNotificationAsync(string phoneNumber, string notification)
        {
            // This method won't work with Verify API
            throw new NotImplementedException("SendNotificationAsync is not supported with Verify API.");
        }

        public async Task SendZaloMessageAsync(string phoneNumber, string message)
        {
            // This method won't work with Verify API
            throw new NotImplementedException("SendZaloMessageAsync is not supported with Verify API.");
        }
    }
} 