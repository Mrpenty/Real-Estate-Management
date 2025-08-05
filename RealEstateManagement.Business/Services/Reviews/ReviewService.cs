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

        public async Task<(bool, string)> AddReviewAsync(int contractId, int propertyId, int renterId, string reviewText, int rating)
        {
            var existed = await _repo.GetReviewByContractAsync(contractId, renterId);
            if (existed != null)
                return (false, "Bạn đã đánh giá hợp đồng này rồi.");

            var review = new Review
            {
                PropertyId = propertyId,
                ContractId = contractId,
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
    }
}
