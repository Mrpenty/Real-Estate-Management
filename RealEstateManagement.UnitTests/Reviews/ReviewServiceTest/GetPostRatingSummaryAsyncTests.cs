using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealEstateManagement.Business.Repositories.Reviews;
using RealEstateManagement.Business.Services.Reviews;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.Reviews;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Reviews.ReviewServiceTest
{
    [TestClass]
    public class GetPostRatingSummaryAsyncTests
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

            // Post 99 + 1 contract + 5 reviews (1..5 sao)
            var post = new PropertyPost
            {
                Id = 99,
                PropertyId = 500,
                LandlordId = 2,
                Status = PropertyPost.PropertyPostStatus.Approved,
                CreatedAt = DateTime.UtcNow
            };
            _db.PropertyPosts.Add(post);

            var contract = new RentalContract
            {
                Id = 9001,
                PropertyPostId = post.Id,
                LandlordId = 2,
                RenterId = 1001,
                DepositAmount = 1,
                MonthlyRent = 1,
                ContractDurationMonths = 12,
                PaymentMethod = "Cash",
                ContactInfo = "Phone",
                StartDate = DateTime.UtcNow.AddMonths(-12),
                Status = RentalContract.ContractStatus.Confirmed
            };
            _db.RentalContracts.Add(contract);

            for (int i = 1; i <= 5; i++)
            {
                _db.Reviews.Add(new Review
                {
                    Id = i,
                    ContractId = contract.Id,
                    PropertyId = post.PropertyId,
                    RenterId = 2000 + i,
                    Rating = i,
                    ReviewText = $"R{i}",
                    CreatedAt = DateTime.UtcNow.AddDays(-i),
                    IsFlagged = false,
                    IsVisible = true
                });
            }

            _db.SaveChanges();

            var repo = new ReviewRepository(_db);
            _svc = new ReviewService(repo);
        }

        [TestMethod]
        public async Task Returns_Correct_Buckets_And_Average()
        {
            var sum = await _svc.GetPostRatingSummaryAsync(99);

            Assert.AreEqual(5, sum.TotalReviews);
            Assert.AreEqual(3.0, sum.AverageRating, 0.001);
            Assert.AreEqual(1, sum.CountStar1);
            Assert.AreEqual(1, sum.CountStar2);
            Assert.AreEqual(1, sum.CountStar3);
            Assert.AreEqual(1, sum.CountStar4);
            Assert.AreEqual(1, sum.CountStar5);
        }

        // 1) Không có review cho post khác
        [TestMethod]
        public async Task Returns_Zero_When_Post_Has_No_Reviews()
        {
            _db.PropertyPosts.Add(new PropertyPost
            {
                Id = 100,
                PropertyId = 600,
                LandlordId = 3,
                Status = PropertyPost.PropertyPostStatus.Approved,
                CreatedAt = DateTime.UtcNow
            });
            _db.SaveChanges();

            var sum = await _svc.GetPostRatingSummaryAsync(100);

            Assert.AreEqual(0, sum.TotalReviews);
            Assert.AreEqual(0.0, sum.AverageRating, 0.0001);
            Assert.AreEqual(0, sum.CountStar1);
            Assert.AreEqual(0, sum.CountStar2);
            Assert.AreEqual(0, sum.CountStar3);
            Assert.AreEqual(0, sum.CountStar4);
            Assert.AreEqual(0, sum.CountStar5);
        }

        // 2) Thêm review mới và kiểm tra bucket/average cập nhật
        [TestMethod]
        public async Task Updates_Buckets_When_Additional_Reviews_Added()
        {
            // Thêm 2 review nữa (1 sao & 5 sao) cho cùng contract/post 99
            _db.Reviews.Add(new Review
            {
                ContractId = 9001,
                PropertyId = 500,
                RenterId = 3001,
                Rating = 5,
                ReviewText = "Extra5",
                CreatedAt = DateTime.UtcNow,
                IsFlagged = false,
                IsVisible = true
            });
            _db.Reviews.Add(new Review
            {
                ContractId = 9001,
                PropertyId = 500,
                RenterId = 3002,
                Rating = 1,
                ReviewText = "Extra1",
                CreatedAt = DateTime.UtcNow,
                IsFlagged = false,
                IsVisible = true
            });
            _db.SaveChanges();

            var sum = await _svc.GetPostRatingSummaryAsync(99);

            // Ban đầu 5 review (1..5), thêm (1,5) => tổng 7, tổng điểm 21 => avg=3
            Assert.AreEqual(7, sum.TotalReviews);
            Assert.AreEqual(3.0, sum.AverageRating, 0.001);
            Assert.AreEqual(2, sum.CountStar1);
            Assert.AreEqual(1, sum.CountStar2);
            Assert.AreEqual(1, sum.CountStar3);
            Assert.AreEqual(1, sum.CountStar4);
            Assert.AreEqual(2, sum.CountStar5);
        }

        // 3) Tất cả review cùng 1 mức sao
        [TestMethod]
        public async Task Returns_Correct_When_All_Reviews_Same_Rating()
        {
            var post = new PropertyPost
            {
                Id = 101,
                PropertyId = 700,
                LandlordId = 5,
                Status = PropertyPost.PropertyPostStatus.Approved,
                CreatedAt = DateTime.UtcNow
            };
            _db.PropertyPosts.Add(post);

            var c = new RentalContract
            {
                Id = 9101,
                PropertyPostId = 101,
                LandlordId = 5,
                RenterId = 5001,
                DepositAmount = 1,
                MonthlyRent = 1,
                ContractDurationMonths = 12,
                PaymentMethod = "Bank",
                ContactInfo = "Mail",
                StartDate = DateTime.UtcNow.AddMonths(-6),
                Status = RentalContract.ContractStatus.Confirmed
            };
            _db.RentalContracts.Add(c);

            // 3 review đều 4 sao
            for (int i = 0; i < 3; i++)
            {
                _db.Reviews.Add(new Review
                {
                    ContractId = c.Id,
                    PropertyId = 700,
                    RenterId = 6000 + i,
                    Rating = 4,
                    ReviewText = $"Same4_{i}",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-i),
                    IsFlagged = false,
                    IsVisible = true
                });
            }
            _db.SaveChanges();

            var sum = await _svc.GetPostRatingSummaryAsync(101);

            Assert.AreEqual(3, sum.TotalReviews);
            Assert.AreEqual(4.0, sum.AverageRating, 0.0001);
            Assert.AreEqual(0, sum.CountStar1);
            Assert.AreEqual(0, sum.CountStar2);
            Assert.AreEqual(0, sum.CountStar3);
            Assert.AreEqual(3, sum.CountStar4);
            Assert.AreEqual(0, sum.CountStar5);
        }

        // 4) Kiểm tra làm tròn Average 2 chữ số
        [TestMethod]
        public async Task Returns_Rounded_Average_To_Two_Decimals()
        {
            var post = new PropertyPost
            {
                Id = 102,
                PropertyId = 800,
                LandlordId = 6,
                Status = PropertyPost.PropertyPostStatus.Approved,
                CreatedAt = DateTime.UtcNow
            };
            _db.PropertyPosts.Add(post);

            var c = new RentalContract
            {
                Id = 9102,
                PropertyPostId = 102,
                LandlordId = 6,
                RenterId = 6001,
                DepositAmount = 1,
                MonthlyRent = 1,
                ContractDurationMonths = 12,
                PaymentMethod = "Bank",
                ContactInfo = "Mail",
                StartDate = DateTime.UtcNow.AddMonths(-8),
                Status = RentalContract.ContractStatus.Confirmed
            };
            _db.RentalContracts.Add(c);

            // (5,4,4) => 13/3 = 4.333333 => round 2 chữ số = 4.33
            _db.Reviews.Add(new Review { ContractId = c.Id, PropertyId = 800, RenterId = 7001, Rating = 5, ReviewText = "r1", CreatedAt = DateTime.UtcNow.AddHours(-3), IsFlagged = false, IsVisible = true });
            _db.Reviews.Add(new Review { ContractId = c.Id, PropertyId = 800, RenterId = 7002, Rating = 4, ReviewText = "r2", CreatedAt = DateTime.UtcNow.AddHours(-2), IsFlagged = false, IsVisible = true });
            _db.Reviews.Add(new Review { ContractId = c.Id, PropertyId = 800, RenterId = 7003, Rating = 4, ReviewText = "r3", CreatedAt = DateTime.UtcNow.AddHours(-1), IsFlagged = false, IsVisible = true });
            _db.SaveChanges();

            var sum = await _svc.GetPostRatingSummaryAsync(102);

            Assert.AreEqual(3, sum.TotalReviews);
            Assert.AreEqual(4.33, sum.AverageRating, 0.001);
            Assert.AreEqual(0, sum.CountStar1);
            Assert.AreEqual(0, sum.CountStar2);
            Assert.AreEqual(0, sum.CountStar3);
            Assert.AreEqual(2, sum.CountStar4);
            Assert.AreEqual(1, sum.CountStar5);
        }
    }
}
