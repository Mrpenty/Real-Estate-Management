using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.OwnerRepo
{
    public class RentalContractRepository : IRentalContractRepository
    {
        private readonly RentalDbContext _context;

        public RentalContractRepository(RentalDbContext context)
        {
            _context = context;
        }

        //Xem hợp đồng của bài Post đó
        public async Task<RentalContract> GetByPostIdAsync(int id)
        {
            return await _context.RentalContracts
                .Include(c => c.PropertyPost)
                .FirstOrDefaultAsync(c => c.PropertyPostId == id);
        }

        public async Task<RentalContract> GetByRentalContractIdAsync(int id)
        {
            return await _context.RentalContracts.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(RentalContract contract)
        {
            contract.CreatedAt = DateTime.UtcNow;
            await _context.RentalContracts.AddAsync(contract);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateContractAsync(RentalContract contract)
        {
            _context.RentalContracts.Update(contract);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contract = await _context.RentalContracts.FindAsync(id);
            if (contract != null)
            {
                _context.RentalContracts.Remove(contract);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateStatusAsync(int contractId, RentalContract.ContractStatus status)
        {
            var contract = await _context.RentalContracts.FindAsync(contractId);
            if (contract == null) throw new Exception("Contract not found");

            contract.Status = status;
            await _context.SaveChangesAsync();
        }
    }

}
