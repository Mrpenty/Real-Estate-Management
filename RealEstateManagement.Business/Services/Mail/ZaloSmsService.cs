using Microsoft.Extensions.Configuration;
using RestSharp;

namespace RealEstateManagement.Business.Services.Mail
{
    //public class ZaloSmsService : ISmsService
    //{
    //    private readonly string _accessToken;
    //    private readonly string _oaId;
    //    private readonly string _apiUrl = "https://business-api.zalo.me/api/oa/message/send";

    //    public ZaloSmsService(IConfiguration configuration)
    //    {
    //        _accessToken = configuration["Zalo:AccessToken"];
    //        _oaId = configuration["Zalo:OAId"];
    //    }

    //    public async Task SendOtpAsync(string phoneNumber, string otp)
    //    {
    //        var client = new RestClient(_apiUrl);
    //        var request = new RestRequest(Method.POST);

    //        // Thêm header
    //        request.AddHeader("Content-Type", "application/json");
    //        request.AddHeader("access_token", _accessToken);

    //        // Dữ liệu gửi đi (theo định dạng API ZNS)
    //        var body = new
    //        {
    //            recipient = new
    //            {
    //                user_id = phoneNumber // Số điện thoại của người nhận (phải là user_id từ Zalo)
    //            },
    //            message = new
    //            {
    //                msgtype = "text",
    //                text = $"Your OTP for [YourAppName] is: {otp}. Valid for 5 minutes."
    //            },
    //            oa_id = _oaId
    //        };

    //        request.AddJsonBody(body);

    //        var response = await client.ExecuteAsync(request);

    //        if (!response.IsSuccessful)
    //        {
    //            throw new Exception($"Failed to send OTP: {response.Content}");
    //        }
    //    }

    //}
}