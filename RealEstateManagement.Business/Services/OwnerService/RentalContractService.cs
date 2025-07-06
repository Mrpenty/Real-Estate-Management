using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.OwnerService
{
    public class RentalContractService : IRentalContractService
    {
        private readonly IRentalContractRepository _repository;

        public RentalContractService(IRentalContractRepository repository)
        {
            _repository = repository;
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

        public async Task AddAsync(RentalContractCreateDto dto)
        {
            // ✅ Kiểm tra dữ liệu đầu vào
            if (dto.PropertyPostId <= 0)
                throw new ArgumentException("Bài đăng bất động sản không hợp lệ.");

            if (dto.LandlordId <= 0)
                throw new ArgumentException("Chủ nhà không hợp lệ.");

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


            var contract = new RentalContract
            {
                PropertyPostId = dto.PropertyPostId,
                LandlordId = dto.LandlordId,
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


            await _repository.AddAsync(contract);
        }

        public async Task UpdateStatusAsync(RentalContractStatusDto statusDto)
        {
            var contract = await _repository.GetByPostIdAsync(statusDto.ContractId);
            if (contract == null)
                throw new Exception("Không tìm thấy hợp đồng.");

            // ✅ Kiểm tra giá trị hợp lệ
            if (!Enum.IsDefined(typeof(RentalContract.ContractStatus), statusDto.Status))
                throw new ArgumentException("Trạng thái hợp đồng không hợp lệ.");

            contract.Status = statusDto.Status;

            if (statusDto.Status == RentalContract.ContractStatus.Confirmed)
            {
                contract.ConfirmedAt = DateTime.Now;
            }

            await _repository.UpdateStatusAsync(contract.Id, contract.Status);
        }

        public async Task UpdateContractAsync(int contractId, RentalContractUpdateDto dto)
        {
            var contract = await _repository.GetByRentalContractIdAsync(contractId);
            if (contract == null)
                throw new Exception("Không tìm thấy hợp đồng.");

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
    }
}
