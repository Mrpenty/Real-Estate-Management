using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Repositories.Chat.Messages;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Data.Entity;
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
        private readonly IMessageRepository _msgReadRepo;
        private readonly IRentalContractRepository _contractRepo; 
        public InterestedPropertyService(IInterestedPropertyRepository repository, IPropertyPostRepository postRepo, IMessageRepository msgReadRepo, IRentalContractRepository contractRepo)
        {
            _repository = repository;
            _postRepo = postRepo;
            _msgReadRepo = msgReadRepo;
            _contractRepo = contractRepo;
        }

        private InterestedPropertyDTO MapToDTO(InterestedProperty entity)
        {
            if (entity == null) return null;
            return new InterestedPropertyDTO
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                RenterId = entity.RenterId,
                InterestedAt = entity.InterestedAt.AddHours(7),
                Status = (int)entity.Status,
                RenterReplyAt = entity.RenterReplyAt,
                LandlordReplyAt = entity.LandlordReplyAt,
                RenterConfirmed = entity.RenterConfirmed,
                LandlordConfirmed = entity.LandlordConfirmed
            };
        }
        private void ResetForReopen(InterestedProperty ip)
        {
            ip.Status = InterestedStatus.WaitingForRenterReply;
            ip.RenterConfirmed = false;
            ip.LandlordConfirmed = false;
            ip.RenterReplyAt = null;
            ip.LandlordReplyAt = null;
            ip.InterestedAt = DateTime.UtcNow;
        }
        // CHANGED: logic xác nhận theo nghiệp vụ mới
        public async Task<bool> ConfirmInterestAsync(int interestedPropertyId, bool isRenter, bool confirmed)
        {
            var ip = await _repository.GetByIdAsync(interestedPropertyId);
            if (ip == null) throw new Exception("Interest not found");

            if (isRenter)
            {
                if (ip.Status != InterestedStatus.WaitingForRenterReply)
                    throw new InvalidOperationException("Không ở bước chờ renter xác nhận.");

                ip.RenterReplyAt = DateTime.UtcNow;      
                ip.RenterConfirmed = confirmed;          

                if (confirmed)
                {
                    ip.Status = InterestedStatus.WaitingForLandlordReply;
                }
                else
                {
                    ip.Status = InterestedStatus.None;    
                    ip.LandlordConfirmed = false;       
                    ip.LandlordReplyAt = null;            
                }

                await _repository.UpdateAsync(ip);
                return true;
            }
            else
            {
                if (ip.Status != InterestedStatus.WaitingForLandlordReply)
                    throw new InvalidOperationException("Không ở bước chờ landlord xác nhận.");

                ip.LandlordReplyAt = DateTime.UtcNow;
                ip.LandlordConfirmed = confirmed; 

                if (!confirmed)
                {
                    ip.Status = InterestedStatus.LandlordRejected;
                    await _repository.UpdateAsync(ip);       
                    return true;
                }

 
                ip.Status = InterestedStatus.DealSuccess;

                if (_postRepo != null)
                {
                    var post = await _postRepo.GetByPropertyIdAsync(ip.PropertyId);
                    if (post != null)
                    {
                        post.Status = PropertyPost.PropertyPostStatus.Rented; 
                        await _postRepo.UpdateAsync(post);

                        var contract = await _contractRepo.GetByPostIdAsync(post.Id);
                        if (contract != null)
                        {
                            contract.Status = RentalContract.ContractStatus.Confirmed;
                            contract.RenterId = ip.RenterId; // ip.RenterId lấy từ InterestedProperty
                            contract.ConfirmedAt = DateTime.UtcNow;
                            await _contractRepo.UpdateContractAsync(contract);
                        }
                    }
                }

                await _repository.UpdateAsync(ip); 

                // CHANGED: đóng tất cả quan tâm khác của cùng property (None)
                var others = await _repository.GetByPropertyAsync(ip.PropertyId); // đã include Renter
                foreach (var other in others.Where(o => o.Id != ip.Id))
                {
                    // Chỉ đóng những bản chưa kết thúc
                    if (other.Status != InterestedStatus.DealSuccess &&
                        other.Status != InterestedStatus.LandlordRejected &&
                        other.Status != InterestedStatus.None)
                    {
                        other.Status = InterestedStatus.None; // CHANGED
                                                              // có thể reset cờ/ thời gian nếu muốn
                        other.RenterConfirmed = false;
                        other.LandlordConfirmed = false;
                        other.RenterReplyAt = null;
                        other.LandlordReplyAt = null;
                        await _repository.UpdateAsync(other); // đơn giản, chấp nhận nhiều lần Save
                    }
                }

                return true;
            }
        }


        // Tạo mới hoặc lấy lại nếu đã quan tâm
        public async Task<InterestedPropertyDTO> AddInterestAsync(int renterId, int propertyId)
        {
            var existing = await _repository.GetByRenterAndPropertyAsync(renterId, propertyId);

            if (existing == null)
            {
                var ip = new InterestedProperty
                {
                    RenterId = renterId,
                    PropertyId = propertyId,
                    InterestedAt = DateTime.UtcNow,
                    Status = InterestedStatus.WaitingForRenterReply
                };
                var added = await _repository.AddAsync(ip);
                return MapToDTO(added);
            }


            if (existing.Status == InterestedStatus.LandlordRejected) 
                throw new InvalidOperationException("Landlord đã từ chối. Không thể quan tâm lại bất động sản này.");

            // Nếu None => cho phép mở lại = WaitingForRenterReply
            if (existing.Status == InterestedStatus.None) // CHANGED
            {
                ResetForReopen(existing);                 // CHANGED
                await _repository.UpdateAsync(existing);  // CHANGED
            }

            if (existing.Status == InterestedStatus.WaitingForRenterReply)
            {
                ResetForReopen(existing);                 // CHANGED
                await _repository.UpdateAsync(existing);  // CHANGED
            }

            // Nếu đang Waiting... hoặc DealSuccess => trả về trạng thái hiện tại
            return MapToDTO(existing);
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

            // Không cho tự ý gán về LandlordRejected/DealSuccess từ ngoài
            if (status == InterestedStatus.LandlordRejected || status == InterestedStatus.DealSuccess) // CHANGED
                throw new InvalidOperationException("Không thể set trực tiếp trạng thái này.");

            ip.Status = status;

            if (status == InterestedStatus.WaitingForRenterReply)
                ip.RenterReplyAt = null;
            else if (status == InterestedStatus.WaitingForLandlordReply)
                ip.LandlordReplyAt = null;
            else if (status == InterestedStatus.None)
            {
                ip.RenterConfirmed = false;
                ip.LandlordConfirmed = false;
            }

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
        private async Task<DateTime> GetLastActivityAsync(InterestedProperty ip, int landlordId)
        {
            var last = ip.InterestedAt;

            if (ip.RenterReplyAt.HasValue && ip.RenterReplyAt.Value > last) last = ip.RenterReplyAt.Value;
            if (ip.LandlordReplyAt.HasValue && ip.LandlordReplyAt.Value > last) last = ip.LandlordReplyAt.Value;

            // lấy lastMessageAt từ conversation (nếu có)
            var (lastMsgAt, _) = await _msgReadRepo
                .GetLastConversationActivityAsync(ip.RenterId, landlordId, ip.PropertyId);

            if (lastMsgAt.HasValue && lastMsgAt.Value > last) last = lastMsgAt.Value;

            return last;
        }
    }

}
