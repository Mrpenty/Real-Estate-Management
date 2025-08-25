using RealEstateManagement.Business.DTO.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Reviews
{
    public interface IReviewService
    {
        Task<(bool IsSuccess, string Message)> AddReviewAsync(int propertyPostId, int renterId, string reviewText, int rating);
        Task<(bool IsSuccess, string Message)> EditReviewAsync(int reviewId, int renterId, string newText);
        Task<(bool IsSuccess, string Message)> AddReplyAsync(int reviewId, int landlordId, string replyContent);
        Task<(bool IsSuccess, string Message)> EditReplyAsync(int replyId, int landlordId, string newContent);
        Task<(bool IsSuccess, string Message)> Report(int commentId);
        Task<bool> DeleteReviewWhenReportResolvedAsync(int reviewId);
            
        Task<PagedResultDTO<ReviewItemDTO>> GetReviewsByPostAsync(int propertyPostId, int page = 1, int pageSize = 10, string sort = "-date");

        Task<PostRatingSummaryDTO> GetPostRatingSummaryAsync(int propertyPostId);
    }
}
