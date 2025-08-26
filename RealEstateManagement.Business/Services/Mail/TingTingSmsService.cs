using Microsoft.Extensions.Configuration;
using RestSharp;
using Newtonsoft.Json;

namespace RealEstateManagement.Business.Services.Mail
{
    //public class ZaloSmsService : ISmsService
    //{
    //    private readonly string _accessToken;
    //    private readonly string _oaId;
    //    private readonly string _apiUrl = "https://business-api.zalo.me/api/oa/message/send";

    //    public ZaloSmsService(IConfiguration configuration)
    //    {
    //        _accessToken = configuration["Zalo:AccessToken"] ?? "RP0bNF72EK4TnqYLN4PX";
    //        _oaId = configuration["Zalo:OAId"] ?? "123456789";
    //        Console.WriteLine($"Zalo SMS Service initialized with Access Token: {_accessToken}");
    //        Console.WriteLine($"Zalo SMS Service initialized with OA ID: {_oaId}");
    //    }

    //    public async Task SendOtpAsync(string phoneNumber, string otp)
    //    {
    //        var message = $"Mã OTP của bạn là: {otp}. Mã có hiệu lực trong 5 phút.";
    //        await SendZaloMessageAsync(phoneNumber, message);
    //    }

    //    public async Task SendSmsAsync(string phoneNumber, string message)
    //    {
    //        await SendZaloMessageAsync(phoneNumber, message);
    //    }

    //    public async Task SendPropertyInquiryAsync(string phoneNumber, string propertyTitle, string contactPhone)
    //    {
    //        var message = $"Bạn có tin nhắn mới về bất động sản: {propertyTitle}. Liên hệ: {contactPhone}";
    //        await SendZaloMessageAsync(phoneNumber, message);
    //    }

    //    public async Task SendNotificationAsync(string phoneNumber, string notification)
    //    {
    //        await SendZaloMessageAsync(phoneNumber, notification);
    //    }

    //    public async Task SendZaloMessageAsync(string phoneNumber, string message)
    //    {
    //        try
    //        {
    //            // Trước tiên, lấy user_id từ số điện thoại
    //            string userId = await GetZaloUserId(phoneNumber);
    //            if (string.IsNullOrEmpty(userId))
    //            {
    //                throw new Exception("Không thể lấy được Zalo user ID cho số điện thoại này.");
    //            }

    //            var client = new RestClient();
    //            var request = new RestRequest(_apiUrl, Method.Post);
                
    //            request.AddHeader("Content-Type", "application/json");
    //            request.AddHeader("access_token", _accessToken);

    //            var body = new
    //            {
    //                recipient = new { user_id = userId },
    //                message = new { 
    //                    msgtype = "text", 
    //                    text = message 
    //                },
    //                oa_id = _oaId
    //            };

    //            Console.WriteLine($"Sending Zalo message to {phoneNumber} (userId: {userId}): {message}");
    //            Console.WriteLine($"Request body: {JsonConvert.SerializeObject(body)}");

    //            request.AddJsonBody(body);
    //            var response = await client.ExecuteAsync(request);

    //            Console.WriteLine($"Response status: {response.StatusCode}");
    //            Console.WriteLine($"Response content: {response.Content}");

    //            if (!response.IsSuccessful)
    //            {
    //                throw new Exception($"Zalo message failed: {response.Content}");
    //            }

    //            var result = JsonConvert.DeserializeObject<dynamic>(response.Content);
    //            if (result?.error != 0)
    //            {
    //                throw new Exception($"Zalo message error: {result?.message ?? "Unknown error"}");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Error sending Zalo message: {ex.Message}");
    //            throw new Exception($"Failed to send message via Zalo: {ex.Message}", ex);
    //        }
    //    }

    //    private async Task<string> GetZaloUserId(string phoneNumber)
    //    {
    //        try
    //        {
    //            var client = new RestClient();
    //            var request = new RestRequest("https://openapi.zalo.me/v2.0/oa/getprofile", Method.Get);
    //            request.AddHeader("access_token", _accessToken);
    //            request.AddParameter("phone", phoneNumber);

    //            Console.WriteLine($"Getting Zalo user ID for phone: {phoneNumber}");

    //            var response = await client.ExecuteAsync(request);
    //            Console.WriteLine($"GetProfile response status: {response.StatusCode}");
    //            Console.WriteLine($"GetProfile response content: {response.Content}");

    //            if (response.IsSuccessful)
    //            {
    //                var data = JsonConvert.DeserializeObject<dynamic>(response.Content);
    //                var userId = data?.data?.user_id;
    //                Console.WriteLine($"Zalo user ID: {userId}");
    //                return userId;
    //            }
                
    //            Console.WriteLine($"Failed to get Zalo user ID: {response.Content}");
    //            return null;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Error getting Zalo user ID: {ex.Message}");
    //            return null;
    //        }
    //    }
    //}
} 