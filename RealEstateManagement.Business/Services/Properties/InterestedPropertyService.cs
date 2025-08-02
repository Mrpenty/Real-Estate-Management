using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Properties
{
    public class InterestedPropertyService : IInterestedPropertyService
    {
        private readonly IInterestedPropertyRepository _repository;
        private readonly IPropertyPostRepository _postRepo; // Giả sử bạn đã inject IPropertyPostRepository để cập nhật trạng thái PropertyPost

        public InterestedPropertyService(IInterestedPropertyRepository repository, IPropertyPostRepository postRepo)
        {
            _repository = repository;
            _postRepo = postRepo;
        }

        private InterestedPropertyDTO MapToDTO(InterestedProperty entity)
        {
            if (entity == null) return null;
            return new InterestedPropertyDTO
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                RenterId = entity.RenterId,
                InterestedAt = entity.InterestedAt,
                Status = (int)entity.Status,
                RenterReplyAt = entity.RenterReplyAt,
                LandlordReplyAt = entity.LandlordReplyAt,
                RenterConfirmed = entity.RenterConfirmed,
                LandlordConfirmed = entity.LandlordConfirmed
            };
        }
        public async Task<bool> ConfirmInterestAsync(int interestedPropertyId, bool isRenter, bool confirmed)
        {
            var ip = await _repository.GetByIdAsync(interestedPropertyId);
            if (ip == null) throw new Exception("Interest not found");

            if (isRenter)
            {
                ip.RenterConfirmed = confirmed;
                ip.RenterReplyAt = DateTime.UtcNow;
                ip.Status = confirmed ? InterestedStatus.RenterWantToRent : InterestedStatus.RenterNotRent;
                // Nếu renter chọn KHÔNG thì kết thúc luôn, không hỏi landlord nữa
                if (!confirmed)
                {
                    ip.LandlordConfirmed = false;
                    ip.LandlordReplyAt = null;
                }
            }
            else
            {
                ip.LandlordConfirmed = confirmed;
                ip.LandlordReplyAt = DateTime.UtcNow;
                ip.Status = confirmed ? InterestedStatus.LandlordAccepted : InterestedStatus.LandlordRejected;
            }

            // Nếu cả 2 đều xác nhận YES thì chuyển trạng thái DealSuccess và PropertyPost sang Rented
            if (ip.RenterConfirmed && ip.LandlordConfirmed)
            {
                ip.Status = InterestedStatus.DealSuccess;
                // Update PropertyPost status (giả sử đã inject IPropertyPostRepository _postRepo)
                if (_postRepo != null)
                {
                    var post = await _postRepo.GetByPropertyIdAsync(ip.PropertyId);
                    if (post != null)
                    {
                        post.Status = PropertyPost.PropertyPostStatus.Rented;
                        await _postRepo.UpdateAsync(post);
                    }
                }
            }

            await _repository.UpdateAsync(ip);
            return true;
        }

        // Tạo mới hoặc lấy lại nếu đã quan tâm
        public async Task<InterestedPropertyDTO> AddInterestAsync(int renterId, int propertyId)
        {
            var existing = await _repository.GetByRenterAndPropertyAsync(renterId, propertyId);
            if (existing != null) return MapToDTO(existing);
            var ip = new InterestedProperty
            {
                RenterId = renterId,
                PropertyId = propertyId,
                InterestedAt = DateTime.UtcNow,
                Status = InterestedStatus.None
            };
            var added = await _repository.AddAsync(ip);
            return MapToDTO(added);
        }

        public async Task<bool> RemoveInterestAsync(int renterId, int propertyId)
        {
            var existing = await _repository.GetByRenterAndPropertyAsync(renterId, propertyId);
            if (existing == null) return false;
            await _repository.DeleteAsync(existing);
            return true;
        }

        public async Task<IEnumerable<InterestedPropertyDTO>> GetByRenterAsync(int renterId)
        {
            var list = await _repository.GetByRenterAsync(renterId);
            return list.Select(MapToDTO);
        }

        public async Task<InterestedPropertyDTO> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return MapToDTO(entity);
        }

        public async Task UpdateStatusAsync(int id, InterestedStatus status)
        {
            var ip = await _repository.GetByIdAsync(id);
            if (ip == null) throw new Exception("Interest not found");
            ip.Status = status;
            if (status == InterestedStatus.RenterWantToRent || status == InterestedStatus.RenterNotRent)
                ip.RenterReplyAt = DateTime.UtcNow;
            else if (status == InterestedStatus.LandlordAccepted || status == InterestedStatus.LandlordRejected)
                ip.LandlordReplyAt = DateTime.UtcNow;
            await _repository.UpdateAsync(ip);
        }

        /// <summary>
        /// Trả về IQueryable cho OData paging/filter/sort
        /// </summary>
        public IQueryable<InterestedPropertyDTO> QueryDTO()
        {
            // Bạn có thể include property/renter nếu cần
            return _repository.Query().Select(x => new InterestedPropertyDTO
            {
                Id = x.Id,
                PropertyId = x.PropertyId,
                RenterId = x.RenterId,
                InterestedAt = x.InterestedAt,
                Status = (int)x.Status,
                RenterReplyAt = x.RenterReplyAt,
                LandlordReplyAt = x.LandlordReplyAt
            });
        }

        /// <summary>
        /// Phân trang thủ công (truyền page, pageSize), trả về PaginatedResponseDTO
        /// </summary>
        public async Task<PaginatedResponseDTO<InterestedPropertyDTO>> GetPaginatedAsync(int page = 1, int pageSize = 10, int? renterId = null)
        {
            var query = _repository.Query();
            if (renterId.HasValue && renterId > 0)
                query = query.Where(x => x.RenterId == renterId.Value);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));

            var data = await query
                .OrderByDescending(x => x.InterestedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = data.Select(MapToDTO).ToList();

            return new PaginatedResponseDTO<InterestedPropertyDTO>
            {
                Data = result,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasNextPage = page < totalPages,
                HasPreviousPage = page > 1
            };
        }
    }

}
