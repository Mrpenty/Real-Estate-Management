using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealEstateManagement.Business.Repositories.Reviews;
using RealEstateManagement.Business.Services.Reviews;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.Reviews;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Reviews.ReviewServiceTest
{
    [TestClass]
    public class GetReviewsByPostAsyncTests
    {
        private RentalDbContext _db = null!;
        private ReviewService _svc = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _db = new RentalDbContext(options);

            // Post (chỉ 1)
            var post = new PropertyPost
            {
                Id = 10,
                PropertyId = 999,
                LandlordId = 2,
                Status = PropertyPost.PropertyPostStatus.Approved,
                CreatedAt = DateTime.UtcNow
            };
            _db.PropertyPosts.Add(post);

            // Renter (nếu repo cần r.Renter != null / Role == "Renter")
            _db.Users.Add(new ApplicationUser
            {
                Id = 101,
                UserName = "renter101",
                Name = "Renter 101",
                Email = "r101@test.com",
                PhoneNumber = "101",
                Role = "Renter",
                CreatedAt = DateTime.UtcNow
            });

            // Hợp đồng DUY NHẤT cho post 10
            var contract = new RentalContract
            {
                Id = 1001,
                PropertyPostId = post.Id,
                LandlordId = 2,
                RenterId = 101,
                DepositAmount = 1,
                MonthlyRent = 1,
                ContractDurationMonths = 12,
                PaymentMethod = "Cash",
                ContactInfo = "Phone",
                StartDate = DateTime.UtcNow.AddMonths(-12),
                Status = RentalContract.ContractStatus.Confirmed
            };
            _db.RentalContracts.Add(contract);

            // 1 review (chỉ set FK)
            _db.Reviews.Add(new Review
            {
                Id = 1,
                ContractId = contract.Id,
                PropertyId = post.PropertyId,
                RenterId = 101,
                Rating = 5,
                ReviewText = "OnlyOne",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                IsFlagged = false,
                IsVisible = true
            });

            _db.SaveChanges();

            var repo = new ReviewRepository(_db);
            _svc = new ReviewService(repo);
        }

        [TestMethod]
        public async Task Return_Reviews_Sorted_ByDate()
        {
            // sanity check theo domain: 1 contract cho 1 post
            Assert.AreEqual(1, _db.RentalContracts.Count(), "Theo domain, mỗi PropertyPost chỉ có 1 hợp đồng.");

            var result = await _svc.GetReviewsByPostAsync(propertyPostId: 10, page: 1, pageSize: 10, sort: "date");

            Assert.AreEqual(1, result.TotalItems, "Should return 1 review for the post.");
            Assert.AreEqual(1, result.Items.Count, "Items count should be 1.");
            Assert.AreEqual("OnlyOne", result.Items[0].ReviewText);
            Assert.AreEqual(5, result.Items[0].Rating);
        }
        [TestMethod]
        public async Task Return_Empty_When_No_Reviews_For_Post()
        {
            // Post khác không có review
            _db.PropertyPosts.Add(new PropertyPost
            {
                Id = 99,
                PropertyId = 1999,
                LandlordId = 3,
                Status = PropertyPost.PropertyPostStatus.Approved,
                CreatedAt = DateTime.UtcNow
            });
            _db.SaveChanges();

            var result = await _svc.GetReviewsByPostAsync(propertyPostId: 99, page: 1, pageSize: 10, sort: "date");

            Assert.AreEqual(0, result.TotalItems, "Should return 0 review for post 99.");
            Assert.AreEqual(0, result.Items.Count);
        }

        [TestMethod]
        public async Task Default_Sort_Is_Descending_By_Date()
        {
            // Gọi với sort = "-date" (mặc định của service cũng là -date)
            var result = await _svc.GetReviewsByPostAsync(propertyPostId: 10, page: 1, pageSize: 10, sort: "-date");

            Assert.AreEqual(1, result.TotalItems);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual("OnlyOne", result.Items[0].ReviewText);
        }

        [TestMethod]
        public async Task Maps_Reply_When_Exists()
        {
            // Thêm reply cho review Id = 1
            _db.ReviewReplies.Add(new ReviewReply
            {
                Id = 500,
                ReviewId = 1,
                LandlordId = 2,
                ReplyContent = "Thanks!",
                CreatedAt = DateTime.UtcNow,
                IsFlagged = false,
                IsVisible = true
            });
            _db.SaveChanges();

            var result = await _svc.GetReviewsByPostAsync(propertyPostId: 10, page: 1, pageSize: 10, sort: "-date");

            Assert.AreEqual(1, result.TotalItems);
            Assert.IsNotNull(result.Items[0].Reply, "Reply should be mapped when exists.");
            Assert.AreEqual("Thanks!", result.Items[0].Reply.ReplyContent);
            Assert.AreEqual(2, result.Items[0].Reply.LandlordId);
        }

    }
}
