using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.PromotionPackageDTO;
using RealEstateManagement.Business.Repositories.Package;
using RealEstateManagement.Data.Entity.Payment;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Properties
{
    public class PropertyPromotionService : IPropertyPromotionService
    {
        private readonly IPropertyPromotionRepository _repo;
        private readonly RentalDbContext _context;
        public PropertyPromotionService(IPropertyPromotionRepository repo, RentalDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<IEnumerable<ViewPropertyPromotionDTO>> GetAllAsync()
        {
            var list = await _repo.GetAsync();
            return list.Select(entity => new ViewPropertyPromotionDTO
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                PackageId = entity.PackageId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = true
            });
        }

        public async Task<ViewPropertyPromotionDTO?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;
            return new ViewPropertyPromotionDTO
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                PackageId = entity.PackageId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = true 
            };
        }

        public async Task<ViewPropertyPromotionDTO> CreateAsync(CreatePropertyPromotionDTO dto, int userId)
        {
            // 1. Lấy Wallet
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null) throw new Exception("Không tìm thấy ví");

            // 2. Lấy Package
            var package = await _context.promotionPackages.FirstOrDefaultAsync(p => p.Id == dto.PackageId && p.IsActive);
            if (package == null) throw new Exception("Không tìm thấy gói hoặc gói không còn hoạt động");

            // 3. Check tiền
            if (wallet.Balance < package.Price)
                throw new Exception("Số dư ví không đủ");

            // 4. Trừ tiền
            wallet.Balance -= package.Price;

            // 5. Tạo PropertyPromotion
            var entity = new PropertyPromotion
            {
                PropertyId = dto.PropertyId,
                PackageId = dto.PackageId,
                StartDate = dto.StartDate,
                EndDate = dto.StartDate + TimeSpan.FromDays(package.DurationInDays),
            };
            await _repo.AddAsync(entity);

            // 6. Log WalletTransaction
            var walletTransaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = -package.Price,
                Type = "Non",
                Description = $"Đăng ký gói {package.Name} cho PropertyId={dto.PropertyId}",
                CreatedAt = DateTime.UtcNow
            };
            _context.WalletTransactions.Add(walletTransaction);

            // 7. Save all
            await _context.SaveChangesAsync();

            // 8. Return DTO
            return new ViewPropertyPromotionDTO
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                PackageId = entity.PackageId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = true 
            };
        }

        public async Task<ViewPropertyPromotionDTO?> UpdateAsync(int id, UpdatePropertyPromotionDTO dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            await _repo.UpdateAsync(existing);
            return new ViewPropertyPromotionDTO
            {
                Id = existing.Id,
                PropertyId = existing.PropertyId,
                PackageId = existing.PackageId,
                StartDate = existing.StartDate,
                EndDate = existing.EndDate,
                IsActive = true 
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
} 