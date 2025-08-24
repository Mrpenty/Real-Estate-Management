using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.API.Controllers.Landlord
{
    [ApiController]
    [Route("api/owner/rental-contracts")]
    public class RentalContractController : ControllerBase
    {
        private readonly IRentalContractService _rentalContractService;

        public RentalContractController(IRentalContractService rentalContractService)
        {
            _rentalContractService = rentalContractService;
        }

        // GET: api/owner/rental-contracts/{propertyPostId}
        [HttpGet("{id}")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> GetByPropertyPostId(int id)
        {
            var contract = await _rentalContractService.GetByPostIdAsync(id);
            return Ok(contract);
        }


        // POST: api/owner/rental-contracts
        [HttpPost("{ownerId}/{propertyPostId}")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> Create([FromBody] RentalContractCreateDto dto, int ownerId, int propertyPostId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _rentalContractService.AddAsync(dto, ownerId, propertyPostId);
                return StatusCode(201);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/owner/rental-contracts/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> UpdateContract(int id, [FromBody] RentalContractUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _rentalContractService.UpdateContractAsync(id, dto);
                return StatusCode(201);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            //return NoContent();
        }

        // PATCH: api/owner/rental-contracts/status
        [HttpPatch("status")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> UpdateStatus([FromBody] RentalContractStatusDto statusDto)
        {
            if (!Enum.IsDefined(typeof(RentalContract.ContractStatus), statusDto.Status))
                return BadRequest("Invalid status.");

            await _rentalContractService.UpdateStatusAsync(statusDto);
            return NoContent();
        }

        // DELETE: api/owner/rental-contracts/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> Delete(int id)
        {
            await _rentalContractService.DeleteAsync(id);
            return NoContent();
        }

        // POST: api/owner/rental-contracts/{id}/pay     
        [HttpPost("{id}/pay")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> Pay(int id)
        {
            try
            {
                var result = await _rentalContractService.PayAsync(id);
                return Ok(result); // result: NextPaymentDate, LateDays
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // PUT: api/owner/rental-contracts/{rentalContractId}/terminate
        [HttpPut("{rentalContractId}/terminate")]
        public async Task<IActionResult> TerminateContract(int rentalContractId)
        {
            var statusDto = new RentalContractStatusDto
            {
                ContractId = rentalContractId,
                Status = RentalContract.ContractStatus.Rejected
            };

            if (!Enum.IsDefined(typeof(RentalContract.ContractStatus), statusDto.Status))
                return BadRequest("Invalid status.");

            await _rentalContractService.UpdateStatusAsync(statusDto);
            return NoContent();
        }

        //Landlord đề xuất gia hạn hợp đồng
        [HttpPost("{id}/renew")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> ProposeRenewal(int id, [FromBody] RentalContractRenewalDto dto)
        {
            await _rentalContractService.ProposeRenewalAsync(id, dto);
            return Ok(new { message = "Đã gửi đề xuất gia hạn" });
        }

        // Renter phản hồi đề xuất gia hạn (approve / reject)
        [HttpPost("{id}/renew/respond")]
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> RespondRenewal(int id, [FromQuery] bool approve)
        {
            await _rentalContractService.RespondRenewalAsync(id, approve);
            return Ok(new { message = approve ? "Đã chấp nhận gia hạn" : "Đã từ chối gia hạn" });
        }

        //Renter xem chi tiết đề xuất gia hạn
        [HttpGet("{id}/renew/proposal")]
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> GetRenewalProposal(int id)
        {
            var proposal = await _rentalContractService.GetRenewalProposalAsync(id);
            if (proposal == null)
                return NotFound(new { message = "Không có đề xuất gia hạn nào cho hợp đồng này" });

            return Ok(proposal);
        }
    }
}
