using Microsoft.Extensions.Logging;

namespace RealEstateManagement.Business.Services.Mail
{
    //public class TestSmsService : ISmsService
    //{
    //    private readonly ILogger<TestSmsService> _logger;

    //    public TestSmsService(ILogger<TestSmsService> logger)
    //    {
    //        _logger = logger;
    //    }

    //    public async Task SendOtpAsync(string phoneNumber, string otp)
    //    {
    //        // Log OTP instead of sending SMS for development
    //        _logger.LogInformation("TEST MODE: OTP for {PhoneNumber} is {Otp}", phoneNumber, otp);
    //        Console.WriteLine($"=== TEST MODE === OTP for {phoneNumber}: {otp} === TEST MODE ===");
            
    //        // Simulate async operation
    //        await Task.Delay(100);
    //    }

    //    public async Task SendSmsAsync(string phoneNumber, string message)
    //    {
    //        _logger.LogInformation("TEST MODE: SMS to {PhoneNumber}: {Message}", phoneNumber, message);
    //        Console.WriteLine($"=== TEST MODE === SMS to {phoneNumber}: {message} === TEST MODE ===");
            
    //        await Task.Delay(100);
    //    }

    //    public async Task SendPropertyInquiryAsync(string phoneNumber, string propertyTitle, string contactPhone)
    //    {
    //        var message = $"Bạn có tin nhắn mới về bất động sản: {propertyTitle}. Liên hệ: {contactPhone}";
    //        await SendSmsAsync(phoneNumber, message);
    //    }

    //    public async Task SendNotificationAsync(string phoneNumber, string notification)
    //    {
    //        await SendSmsAsync(phoneNumber, notification);
    //    }

    //    public async Task SendZaloMessageAsync(string phoneNumber, string message)
    //    {
    //        await SendSmsAsync(phoneNumber, message);
    //    }
    //}
} 