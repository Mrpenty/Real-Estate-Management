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

        public async Task<RentalContract> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<RentalContract>> GetByPostIdAsync(int postId)
        {
            return await _repository.GetByPostIdAsync(postId);
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
                Status = RentalContract.ContractStatus.Pending,
                CreatedAt = DateTime.Now
            };

            await _repository.AddAsync(contract);
        }

        public async Task UpdateStatusAsync(RentalContractStatusDto statusDto)
        {
            var contract = await _repository.GetByIdAsync(statusDto.ContractId);
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
            var contract = await _repository.GetByIdAsync(contractId);
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

            contract.DepositAmount = dto.DepositAmount;
            contract.MonthlyRent = dto.MonthlyRent;
            contract.ContractDurationMonths = dto.ContractDurationMonths;
            contract.StartDate = dto.StartDate;
            contract.PaymentMethod = dto.PaymentMethod;
            contract.ContactInfo = dto.ContactInfo;

            await _repository.UpdateContractAsync(contract);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
