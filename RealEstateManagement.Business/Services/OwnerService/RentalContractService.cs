using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RealEstateManagement.Data.Entity.PropertyEntity.PropertyPost;

namespace RealEstateManagement.Business.Services.OwnerService
{
    public class RentalContractService : IRentalContractService
    {
        private readonly IRentalContractRepository _repository;
        private readonly IPropertyPostRepository _propertyPostRepo;
        private readonly IProfileService _user;
        private readonly IMailService _mailService;

        public RentalContractService(IRentalContractRepository repository, IPropertyPostRepository propertyPost, IProfileService profileService, IMailService mailService)
        {
            _repository = repository;
            _propertyPostRepo = propertyPost;
            _user = profileService;
            _mailService = mailService;
        }

        //Xem hợp đồng của bài Post đó
        public async Task<RentalContractViewDto> GetByPostIdAsync(int id)
        {
            var entity = await _repository.GetByPostIdAsync(id);
            if (entity == null) throw new Exception("Không tìm thấy hợp đồng.");

            return new RentalContractViewDto
            {
                Id = entity.Id,
                PropertyPostId = entity.PropertyPostId,
                DepositAmount = entity.DepositAmount,
                MonthlyRent = entity.MonthlyRent,
                ContractDurationMonths = entity.ContractDurationMonths,
                PaymentCycle = entity.PaymentCycle,
                PaymentDayOfMonth = entity.PaymentDayOfMonth,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                PaymentMethod = entity.PaymentMethod,
                ContactInfo = entity.ContactInfo,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                ConfirmedAt = entity.ConfirmedAt
            };
        }

        public async Task AddAsync(RentalContractCreateDto dto, int ownerId, int propertyPostId)
        {
            // ✅ Kiểm tra dữ liệu đầu vào

            if (dto.DepositAmount < 0 || dto.MonthlyRent <= 0)
                throw new ArgumentException("Số tiền đặt cọc hoặc tiền thuê hàng tháng không hợp lệ.");

            if (dto.ContractDurationMonths <= 0)
                throw new ArgumentException("Thời hạn hợp đồng phải lớn hơn 0.");

            if (string.IsNullOrWhiteSpace(dto.ContactInfo))
                throw new ArgumentException("Thông tin liên hệ không được để trống.");

            if (string.IsNullOrWhiteSpace(dto.PaymentMethod))
                throw new ArgumentException("Phương thức thanh toán không được để trống.");

            if (dto.PaymentDayOfMonth < 1 || dto.PaymentDayOfMonth > 31)
                throw new ArgumentException("Ngày thanh toán phải nằm trong khoảng từ 1 đến 31.");

            if (!Enum.IsDefined(typeof(RentalContract.PaymentCycleType), dto.PaymentCycle))
                throw new ArgumentException("Kiểu thanh toán không hợp lệ (phải là Monthly, Quarterly hoặc Yearly).");

            var propertyPost = await _propertyPostRepo.GetPropertyPostByIdAsync(propertyPostId, ownerId);
            if (propertyPost == null)
                throw new ArgumentException("Không tìm thấy bài đăng bất động sản hợp lệ cho chủ nhà này.");

            var contract = new RentalContract
            {
                PropertyPostId = propertyPostId,
                LandlordId = ownerId,
                DepositAmount = dto.DepositAmount,
                MonthlyRent = dto.MonthlyRent,
                ContractDurationMonths = dto.ContractDurationMonths,
                StartDate = dto.StartDate,
                PaymentMethod = dto.PaymentMethod,
                ContactInfo = dto.ContactInfo,
                PaymentCycle = dto.PaymentCycle,
                PaymentDayOfMonth = dto.PaymentDayOfMonth,
                Status = RentalContract.ContractStatus.Pending,
                CreatedAt = DateTime.Now
            };

            propertyPost.Status = PropertyPost.PropertyPostStatus.Pending;
            propertyPost.UpdatedAt = DateTime.UtcNow;
            await _propertyPostRepo.UpdateAsync(propertyPost);
            await _repository.AddAsync(contract,ownerId, propertyPostId);
        }

        public async Task UpdateStatusAsync(RentalContractStatusDto statusDto)
        {
            var contract = await _repository.GetByPostIdAsync(statusDto.ContractId);
            if (contract == null)
                throw new Exception("Không tìm thấy hợp đồng.");

            var newStatus = (RentalContract.ContractStatus)statusDto.Status;
            // ✅ Kiểm tra giá trị hợp lệ
            if (!Enum.IsDefined(typeof(RentalContract.ContractStatus), statusDto.Status))
                throw new ArgumentException("Trạng thái hợp đồng không hợp lệ.");

            contract.Status = newStatus;

            if (newStatus == RentalContract.ContractStatus.Confirmed)
            {
                contract.ConfirmedAt = DateTime.Now;
            }

            await _repository.UpdateStatusAsync(contract.Id, contract.Status);
        }

        public async Task<bool> TerminateContractAsync(int contractId)
        {
            var contract = await _repository.GetByRentalContractIdAsync(contractId);

            if (contract == null) return false;

            contract.Status = RentalContract.ContractStatus.Terminated;

            await _repository.UpdateStatusAsync(contract.Id, contract.Status);
            return true;
        }

        public async Task UpdateContractAsync(int contractId, RentalContractUpdateDto dto)
        {
            var contract = await _repository.GetByRentalContractIdAsync(contractId);
            if (contract == null)
                throw new ArgumentException("Không tìm thấy hợp đồng.");

            // ✅ Kiểm tra dữ liệu
            if (dto.DepositAmount < 0 || dto.MonthlyRent <= 0)
                throw new ArgumentException("Số tiền đặt cọc hoặc tiền thuê hàng tháng không hợp lệ.");

            if (dto.ContractDurationMonths <= 0)
                throw new ArgumentException("Thời hạn hợp đồng phải lớn hơn 0.");

            if (string.IsNullOrWhiteSpace(dto.ContactInfo))
                throw new ArgumentException("Thông tin liên hệ không được để trống.");

            if (string.IsNullOrWhiteSpace(dto.PaymentMethod))
                throw new ArgumentException("Phương thức thanh toán không được để trống.");

            if (dto.PaymentDayOfMonth < 1 || dto.PaymentDayOfMonth > 31)
                throw new ArgumentException("Ngày thanh toán phải nằm trong khoảng từ 1 đến 31.");

            if (!Enum.IsDefined(typeof(RentalContract.PaymentCycleType), dto.PaymentCycle))
                throw new ArgumentException("Kiểu thanh toán không hợp lệ (phải là Monthly, Quarterly hoặc Yearly).");


            contract.DepositAmount = dto.DepositAmount;
            contract.MonthlyRent = dto.MonthlyRent;
            contract.ContractDurationMonths = dto.ContractDurationMonths;
            contract.StartDate = dto.StartDate;
            contract.PaymentMethod = dto.PaymentMethod;
            contract.ContactInfo = dto.ContactInfo;
            contract.PaymentCycle = dto.PaymentCycle;
            contract.PaymentDayOfMonth = dto.PaymentDayOfMonth;

            await _repository.UpdateContractAsync(contract);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<RentalPaymentResultDto> PayAsync(int contractId)
        {
            var contract = await _repository.GetByRentalContractIdAsync(contractId);
            if (contract == null) throw new ArgumentException("Contract not found");

            if (contract.StartDate == null) throw new ArgumentException("Contract chưa có ngày bắt đầu");

            // Tính ngày đến hạn hiện tại
            var now = DateTime.Now;
            DateTime due = CalculateNextPaymentDate(contract);

            if (now < due)
            {
                throw new ArgumentException("Chưa đến hạn thanh toán");
            }

            int lateDays = 0;
            if (now > due)
            {
                lateDays = (now - due).Days;
            }

            // cập nhật LastPaymentDate
            contract.LastPaymentDate = now;
            await _repository.UpdateContractAsync(contract);

            // Trả về kết quả
            return new RentalPaymentResultDto
            {
                ContractId = contract.Id,
                LastPaymentDate = now,
                NextPaymentDate = GetNextCycleDate(due, contract.PaymentCycle, contract.PaymentDayOfMonth),
                LateDays = lateDays
            };
        }

        private DateTime CalculateNextPaymentDate(RentalContract contract)
        {
            var start = contract.StartDate.Value;
            var due = new DateTime(start.Year, start.Month, contract.PaymentDayOfMonth);

            while (due <= DateTime.Now)
            {
                due = GetNextCycleDate(due, contract.PaymentCycle, contract.PaymentDayOfMonth);
            }
            return due;
        }

        private DateTime GetNextCycleDate(DateTime date, RentalContract.PaymentCycleType cycle, int paymentDay)
        {
            DateTime next = date;
            if (cycle == RentalContract.PaymentCycleType.Monthly)
                next = date.AddMonths(1);
            else if (cycle == RentalContract.PaymentCycleType.Quarterly)
                next = date.AddMonths(3);
            else if (cycle == RentalContract.PaymentCycleType.Yearly)
                next = date.AddYears(1);

            return new DateTime(next.Year, next.Month, paymentDay);
        }

        // ✅ Landlord đề xuất gia hạn
        public async Task ProposeRenewalAsync(int contractId, RentalContractRenewalDto dto)
        {
            var contract = await _repository.GetByRentalContractIdAsync(contractId);
            if (contract == null) throw new ArgumentException("Không tìm thấy hợp đồng");

            contract.ProposedMonthlyRent = dto.ProposedMonthlyRent;
            contract.ProposedContractDurationMonths = dto.ProposedContractDurationMonths;
            contract.ProposedPaymentCycle = dto.ProposedPaymentCycle;
            contract.ProposedPaymentDayOfMonth = dto.ProposedPaymentDayOfMonth;
            contract.ProposedEndDate = dto.ProposedEndDate;
            contract.ProposedAt = DateTime.Now;
            contract.Status = RentalContract.ContractStatus.RenewalPending;
            contract.RenterApproved = null;

            var property = await _propertyPostRepo.GetPostWithPropertyAsync(contract.PropertyPostId);
            var renter = await _user.GetUserBasicInfoAsync(contract.RenterId ?? 0);
            var emailBody1 = $@"
                    <html><body>
                      <p>Xin chào {renter.Name},</p>
                      <p>Chủ nhà vừa đề xuất gia hạn hợp đồng của bất động sản <b>{property.Property.Title}</b>.</p>
                      <p>Bạn hãy vào trang web để xem các yêu cầu đề xuất.</p>
                      <br/>
                      <p>Trân trọng,<br/>BĐS Management</p>
                    </body></html>";
            await _mailService.SendEmailAsync(
            renter.Email,
            "Chủ nhà từ chối cho thuê",
            emailBody1
            );

            await _repository.UpdateContractAsync(contract);
        }

        // ✅ Renter phản hồi
        public async Task RespondRenewalAsync(int contractId, bool approve)
        {
            var contract = await _repository.GetByRentalContractIdAsync(contractId);
            if (contract == null) throw new ArgumentException("Không tìm thấy hợp đồng");

            contract.RenterApproved = approve;

            if (approve)
            {
                contract.MonthlyRent = contract.ProposedMonthlyRent ?? contract.MonthlyRent;
                contract.ContractDurationMonths = contract.ProposedContractDurationMonths ?? contract.ContractDurationMonths;
                contract.PaymentCycle = contract.ProposedPaymentCycle ?? contract.PaymentCycle;
                contract.PaymentDayOfMonth = contract.ProposedPaymentDayOfMonth ?? contract.PaymentDayOfMonth;
                contract.Status = RentalContract.ContractStatus.Confirmed;
                if (contract.ProposedEndDate.HasValue)
                    contract.StartDate = contract.StartDate ?? DateTime.UtcNow;
                contract.RenterApproved = true;
            }
            else
            {
                if (contract.Status == RentalContract.ContractStatus.RenewalPending)
                {
                    // kiểm tra status trước đó
                    if (contract.EndDate.HasValue && contract.EndDate.Value < DateTime.Now)
                    {
                        contract.Status = RentalContract.ContractStatus.Expired;
                    }
                    else
                    {
                        contract.Status = RentalContract.ContractStatus.Confirmed;
                    }
                }
            }

            await _repository.UpdateContractAsync(contract);
        }

        public async Task<RentalContractRenewalDto?> GetRenewalProposalAsync(int contractId)
        {
            var contract = await _repository.GetByRentalContractIdAsync(contractId);
            if (contract == null || contract.Status != RentalContract.ContractStatus.RenewalPending)
                return null;

            return new RentalContractRenewalDto
            {
                ProposedMonthlyRent = contract.ProposedMonthlyRent ?? contract.MonthlyRent,
                ProposedContractDurationMonths = contract.ProposedContractDurationMonths ?? contract.ContractDurationMonths,
                ProposedPaymentCycle = contract.ProposedPaymentCycle ?? contract.PaymentCycle,
                ProposedPaymentDayOfMonth = contract.ProposedPaymentDayOfMonth ?? contract.PaymentDayOfMonth,
                ProposedEndDate = contract.ProposedEndDate ?? contract.EndDate,
                ProposedAt = contract.ProposedAt,
                RenterApproved = contract.RenterApproved
            };
        }

    }
}
