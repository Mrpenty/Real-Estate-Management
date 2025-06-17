namespace RealEstateManagement.Business.DTO.UserDTO
{
    public class UpdateProfileDto
    {
        public string Name { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } 
        public string PhoneNumber { get; set; }
    }
}
