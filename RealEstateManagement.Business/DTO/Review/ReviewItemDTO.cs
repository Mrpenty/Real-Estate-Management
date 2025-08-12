namespace RealEstateManagement.Business.DTO.Review
{
    public class ReviewItemDTO
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RenterId { get; set; }
        public string RenterName { get; set; }
        public ReviewReplyDTO Reply { get; set; }
    }
}
