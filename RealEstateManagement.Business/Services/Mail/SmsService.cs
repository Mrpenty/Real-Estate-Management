using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using RestSharp;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;

namespace RealEstateManagement.Business.Services.Mail
{
    public class SmsService : ISmsService
    {
        // Twilio configuration
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _twilioPhoneNumber;
        // Zalo configuration 
        private readonly string _accessToken;
        private readonly string _oaId;
        private readonly string _apiUrl = "https://business-api.zalo.me/api/oa/message/send";



        public SmsService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _twilioPhoneNumber = configuration["Twilio:PhoneNumber"];
            TwilioClient.Init(_accountSid, _authToken);

            _accessToken = configuration["Zalo:AccessToken"];
            _oaId = configuration["Zalo:OAId"];
        }

        public async Task SendOtpAsync(string phoneNumber, string otp)
        {
            var message = await MessageResource.CreateAsync(
                from: new Twilio.Types.PhoneNumber(_twilioPhoneNumber),
                to: new Twilio.Types.PhoneNumber(phoneNumber),
                body: $"Your OTP is: {otp}. Valid for 5 minutes."
            );
        }


        public async Task SendOtpBothAsync(string phoneNumber, string otp)
        {
            try
            {
                // Thử gửi qua Zalo
                string userId = await GetZaloUserId(phoneNumber);
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("Cannot get Zalo user ID for the provided phone number.");
                }

                var client = new RestClient();
                var request = new RestRequest(_apiUrl, Method.Post); // Sửa tại dòng 57
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("access_token", _accessToken);

                var body = new
                {
                    recipient = new { user_id = userId },
                    message = new { msgtype = "text", text = $"Your OTP for [YourAppName] is: {otp}. Valid for 5 minutes." },
                    oa_id = _oaId
                };

                request.AddJsonBody(body);
                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Zalo OTP failed: {response.Content}");
                }
            }
            catch (Exception)
            {
                // Failover sang Twilio
                var message = await MessageResource.CreateAsync(
                    from: new Twilio.Types.PhoneNumber(_twilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(phoneNumber),
                    body: $"Your OTP is: {otp}. Valid for 5 minutes."
                );

                if (message.Status != MessageResource.StatusEnum.Sent && message.Status != MessageResource.StatusEnum.Delivered)
                {
                    throw new Exception($"Failed to send OTP via Twilio: {message.ErrorMessage}");
                }
            }
        }

        public async Task TwilioSmsService(string phoneNumber, string otp)
        {
            var message = await MessageResource.CreateAsync(
                from: new Twilio.Types.PhoneNumber(_twilioPhoneNumber),
                to: new Twilio.Types.PhoneNumber(phoneNumber),
                body: $"Your OTP is: {otp}. Valid for 5 minutes."
            );

            if (message.Status != MessageResource.StatusEnum.Sent && message.Status != MessageResource.StatusEnum.Delivered)
            {
                throw new Exception($"Failed to send OTP via Twilio: {message.ErrorMessage}");
            }
        }

        public async Task ZaloSmsService(string phoneNumber, string otp)
        {
            string userId = await GetZaloUserId(phoneNumber);
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Cannot get Zalo user ID for the provided phone number.");
            }

            var client = new RestClient();
            var request = new RestRequest(_apiUrl, Method.Post); // Sửa tại dòng 115
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", _accessToken);

            var body = new
            {
                recipient = new { user_id = userId },
                message = new { msgtype = "text", text = $"Your OTP for [YourAppName] is: {otp}. Valid for 5 minutes." },
                oa_id = _oaId
            };

            request.AddJsonBody(body);
            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to send OTP via Zalo: {response.Content}");
            }
        }

        private async Task<string> GetZaloUserId(string phoneNumber)
        {
            var client = new RestClient();
            var request = new RestRequest("https://openapi.zalo.me/v2.0/oa/getprofile", Method.Get); // Sửa tại dòng 138
            request.AddHeader("access_token", _accessToken);
            request.AddParameter("phone", phoneNumber); 

            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                var data = JsonConvert.DeserializeObject<dynamic>(response.Content);
                return data?.user_id; 
            }
            return null;
        }
    }
}