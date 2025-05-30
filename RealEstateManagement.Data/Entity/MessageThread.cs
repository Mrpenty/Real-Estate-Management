namespace RealEstateManagement.Data.Entity
{
    public class MessageThread
    {
      
            public int Id { get; set; }
            public int ThreadId { get; set; }
            public int RenterId { get; set; }
            public int LandlordId { get; set; }
            public int SenderId { get; set; }
            public string Content { get; set; }
            public bool IsRead { get; set; }
            public bool NotificationSent { get; set; }
            public DateTime SentAt { get; set; }

            public ApplicationUser Renter { get; set; }
            public ApplicationUser Landlord { get; set; }
            public ApplicationUser Sender { get; set; }
        
    }
}
