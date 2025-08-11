namespace RealEstateManagement.Business.DTO.Review
{
    public class PostRatingSummaryDTO
    {
        public int PropertyPostId { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; } // 0-5
        public int CountStar1 { get; set; }
        public int CountStar2 { get; set; }
        public int CountStar3 { get; set; }
        public int CountStar4 { get; set; }
        public int CountStar5 { get; set; }
    }
}
