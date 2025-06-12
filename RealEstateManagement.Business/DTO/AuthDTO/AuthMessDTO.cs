namespace RealEstateManagement.Business.DTO.AuthDTO
{
    public class AuthMessDTO
    {
        public bool IsAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }

        public string Token { get; set; }
    }
}
