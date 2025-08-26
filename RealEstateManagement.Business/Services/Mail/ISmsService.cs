using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Mail
{
    public interface ISmsService
    {
        Task SendOtpAsync(string phoneNumber, string otp);
        Task<bool> VerifyOtpAsync(string phoneNumber, string otp);
        Task SendSmsAsync(string phoneNumber, string message);
        Task SendPropertyInquiryAsync(string phoneNumber, string propertyTitle, string contactPhone);
        Task SendNotificationAsync(string phoneNumber, string notification);
        Task SendZaloMessageAsync(string phoneNumber, string message);
    }
}
