using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.API.Controllers.Owner
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

        // GET: api/owner/rental-contracts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _rentalContractService.GetByIdAsync(id);
            if (contract == null)
                return NotFound();

            return Ok(contract);
        }

        // GET: api/owner/rental-contracts/post/{postId}
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetByPostId(int postId)
        {
            var contracts = await _rentalContractService.GetByPostIdAsync(postId);
            return Ok(contracts);
        }

        // POST: api/owner/rental-contracts
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RentalContractCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _rentalContractService.AddAsync(dto);
            return StatusCode(201);
        }

        // PUT: api/owner/rental-contracts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContract(int id, [FromBody] RentalContractUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _rentalContractService.UpdateContractAsync(id, dto);
            return NoContent();
        }

        // PATCH: api/owner/rental-contracts/status
        [HttpPatch("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] RentalContractStatusDto statusDto)
        {
            if (!Enum.IsDefined(typeof(RentalContract.ContractStatus), statusDto.Status))
                return BadRequest("Invalid status.");

            await _rentalContractService.UpdateStatusAsync(statusDto);
            return NoContent();
        }

        // DELETE: api/owner/rental-contracts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _rentalContractService.DeleteAsync(id);
            return NoContent();
        }
    }
}
