namespace RealEstateManagement.Business.DTO.UserDTO
{
    public class ViewProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public string? CitizenIdFrontImageUrl { get; set; }
        public string? CitizenIdBackImageUrl { get; set; }
        public string? CitizenIdNumber { get; set; }
        public DateTime? CitizenIdIssuedDate { get; set; }
        public DateTime? CitizenIdExpiryDate { get; set; }

        public string? VerificationRejectReason { get; set; }
        public string VerificationStatus { get; set; } = "none";
    }
}
