using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Properties
{
    public class HomePropertyDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int AddressID { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public decimal Price { get; set; }
        public string? Status { get; set; }
        public string Location { get; set; }
        public string? PrimaryImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ViewsCount { get; set; }
        public int LandlordId { get; set; }
        public string LandlordName { get; set; }
        public string LandlordPhoneNumber { get; set; }
        public string LandlordProfilePictureUrl { get; set; }
        //Gói vip
        public string? PromotionPackageName { get; set; }
        public List<string> Amenities { get; set; }

        // thêm
        public int? ProvinceId { get; set; }
        public string Province { get; set; }
        public int? WardId { get; set; }
        public string Ward { get; set; }
        public int? StreetId { get; set; }
        public string Street { get; set; }
        public string DetailedAddress { get; set; }
        public bool IsFavorite { get; set; }
        //Property Post
        public int PropertyPostId { get; set; }
        public int InterestedId { get; set; }
        public bool IsInterested { get; set; }
        public InterestedStatus InterestedStatus { get; set; }
        public float? Rating { get; set; }
        public int? RatingNo { get; set; }
        public int IsReminderRenterConfirmInterested { get; set; }
        //0 => none
        //1 => reminder
        //2 => expire
    }
}
