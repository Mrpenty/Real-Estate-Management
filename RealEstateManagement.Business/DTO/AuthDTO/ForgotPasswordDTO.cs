namespace RealEstateManagement.Business.DTO.AuthDTO
{
    public class ForgotPasswordDTO
    {
        public string PhoneNumber { get; set; }
    }

    public class ResetPasswordWithOTPDTO
    {
        public string PhoneNumber { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
