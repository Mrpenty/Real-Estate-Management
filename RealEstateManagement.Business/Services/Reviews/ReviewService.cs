using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.Review;
using RealEstateManagement.Business.Repositories.Reviews;
using RealEstateManagement.Data.Entity.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repo;
        public ReviewService(IReviewRepository repo) => _repo = repo;

        public async Task<(bool, string)> AddReviewAsync(int propertyPostId, int renterId, string reviewText, int rating)
        {
            // 1. Lấy hợp đồng đã hoàn thành của renter với propertyPost này
            var contract = await _repo.GetCompletedContractAsync(propertyPostId, renterId);
            if (contract == null)
                return (false, "Bạn không đủ điều kiện review: hợp đồng chưa hoàn thành!");

            // 2. Đã review hợp đồng này chưa?
            var existed = await _repo.GetReviewByContractAsync(contract.Id, renterId);
            if (existed != null)
                return (false, "Bạn đã đánh giá hợp đồng này rồi.");

            // 3. Tạo review
            var review = new Review
            {
                PropertyId = contract.PropertyPost.PropertyId, // Lấy chuẩn qua propertyPost
                ContractId = contract.Id,
                RenterId = renterId,
                ReviewText = reviewText,
                Rating = rating,
                CreatedAt = DateTime.Now,
                IsFlagged = false,
                IsVisible = true,
            };
            await _repo.AddReviewAsync(review);
            await _repo.SaveChangesAsync();
            return (true, "Thành công");
        }

        public async Task<(bool, string)> EditReviewAsync(int reviewId, int renterId, string newText)
        {
            var review = await _repo.GetReviewByIdAsync(reviewId);
            if (review == null || review.RenterId != renterId)
                return (false, "Không tìm thấy review.");

            if (review.Reply != null)
                return (false, "Chủ nhà đã trả lời, không thể chỉnh sửa.");

            if ((DateTime.Now - review.CreatedAt).TotalMinutes > 5)
                return (false, "Chỉ được sửa trong 5 phút đầu.");

            review.ReviewText = newText;
            await _repo.UpdateReviewAsync(review);
            await _repo.SaveChangesAsync();
            return (true, "Cập nhật thành công");
        }

        public async Task<(bool, string)> AddReplyAsync(int reviewId, int landlordId, string replyContent)
        {
            var review = await _repo.GetReviewByIdAsync(reviewId);
            if (review == null)
                return (false, "Không tìm thấy review.");

            if (review.Reply != null)
                return (false, "Bạn chỉ được trả lời 1 lần.");

            var reply = new ReviewReply
            {
                ReviewId = reviewId,
                LandlordId = landlordId,
                ReplyContent = replyContent,
                CreatedAt = DateTime.Now,
                IsFlagged = false,
                IsVisible = true
            };
            await _repo.AddReplyAsync(reply);
            await _repo.SaveChangesAsync();
            return (true, "Trả lời thành công");
        }

        public async Task<(bool, string)> EditReplyAsync(int replyId, int landlordId, string newContent)
        {
            var reply = await _repo.GetReplyByReviewIdAsync(replyId);
            if (reply == null || reply.LandlordId != landlordId)
                return (false, "Không tìm thấy trả lời hoặc không có quyền.");

            if ((DateTime.Now - reply.CreatedAt).TotalMinutes > 5)
                return (false, "Chỉ được sửa trong 5 phút đầu.");

            reply.ReplyContent = newContent;
            await _repo.UpdateReplyAsync(reply);
            await _repo.SaveChangesAsync();
            return (true, "Cập nhật trả lời thành công");
        }

        public async Task<bool> DeleteReviewWhenReportResolvedAsync(int reviewId)
        {
            return await _repo.HardDeleteReviewAsync(reviewId);
        }
        public async Task<PagedResultDTO<ReviewItemDTO>> GetReviewsByPostAsync(int propertyPostId, int page = 1, int pageSize = 10, string sort = "-date")
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 100) pageSize = 10;

            var q = _repo.QueryReviewsByPropertyPostId(propertyPostId);

            // sort: "-date" (mới nhất), "date" (cũ nhất), "-rating" (sao cao trước), "rating" (sao thấp trước)
            q = sort switch
            {
                "date" => q.OrderBy(r => r.CreatedAt),
                "-rating" => q.OrderByDescending(r => r.Rating).ThenByDescending(r => r.CreatedAt),
                "rating" => q.OrderBy(r => r.Rating).ThenByDescending(r => r.CreatedAt),
                _ => q.OrderByDescending(r => r.CreatedAt),
            };

            var total = await q.CountAsync();

            var items = await q
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new ReviewItemDTO
                {
                    ReviewId = r.Id,
                    Rating = r.Rating,
                    ReviewText = r.ReviewText,
                    CreatedAt = r.CreatedAt,
                    RenterId = r.RenterId,
                    RenterName = r.Renter != null ? (r.Renter.Name ?? r.Renter.UserName) : $"Renter#{r.RenterId}",
                    Reply = r.Reply == null ? null : new ReviewReplyDTO
                    {
                        Id = r.Reply.Id,
                        LandlordId = r.Reply.LandlordId,
                        ReplyContent = r.Reply.ReplyContent,
                        IsFlagged = r.IsFlagged,
                        CreatedAt = r.Reply.CreatedAt
                    }
                })
                .ToListAsync();

            return new PagedResultDTO<ReviewItemDTO>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task<PostRatingSummaryDTO> GetPostRatingSummaryAsync(int propertyPostId)
        {
            var q = _repo.QueryReviewsByPropertyPostId(propertyPostId);

            var total = await q.CountAsync();
            var avg = total == 0 ? 0 : await q.AverageAsync(r => (double)r.Rating);

            // Đếm theo sao
            var buckets = await q
                .GroupBy(r => r.Rating)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .ToListAsync();

            int c1 = buckets.Where(b => b.Rating == 1).Select(b => b.Count).FirstOrDefault();
            int c2 = buckets.Where(b => b.Rating == 2).Select(b => b.Count).FirstOrDefault();
            int c3 = buckets.Where(b => b.Rating == 3).Select(b => b.Count).FirstOrDefault();
            int c4 = buckets.Where(b => b.Rating == 4).Select(b => b.Count).FirstOrDefault();
            int c5 = buckets.Where(b => b.Rating == 5).Select(b => b.Count).FirstOrDefault();

            return new PostRatingSummaryDTO
            {
                PropertyPostId = propertyPostId,
                TotalReviews = total,
                AverageRating = Math.Round(avg, 2),
                CountStar1 = c1,
                CountStar2 = c2,
                CountStar3 = c3,
                CountStar4 = c4,
                CountStar5 = c5
            };
        }

        public async Task<(bool IsSuccess, string Message)> Report(int commentId)
        {
            var report = await _repo.GetReviewByIdAsync(commentId);
            if(report == null) return (false, "Không có bình luận");
            if(report.IsFlagged) return (false, "Bình luận đã được báo cáo trước đó");
            report.IsFlagged = true;
            await _repo.SaveChangesAsync();
            return (true, "thành công");
        }
    }
}
