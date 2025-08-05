using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Reviews
{
    public interface IReviewService
    {
        Task<(bool IsSuccess, string Message)> AddReviewAsync(int contractId, int propertyId, int renterId, string reviewText, int rating);
        Task<(bool IsSuccess, string Message)> EditReviewAsync(int reviewId, int renterId, string newText);
        Task<(bool IsSuccess, string Message)> AddReplyAsync(int reviewId, int landlordId, string replyContent);
        Task<(bool IsSuccess, string Message)> EditReplyAsync(int replyId, int landlordId, string newContent);
        Task<bool> DeleteReviewWhenReportResolvedAsync(int reviewId);
    }
}
