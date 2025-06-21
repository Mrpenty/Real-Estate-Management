using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Mail
{
    public interface ISmsService
    {
        Task TwilioSmsService(string phoneNumber, string otp);
        Task ZaloSmsService(string phoneNumber, string otp);
    }
}
