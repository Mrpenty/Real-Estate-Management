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
    }
}
