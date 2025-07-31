using Microsoft.AspNetCore.Http;
using RealEstateManagement.Business.DTO.SliderDTO;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISliderService
{
    Task<IEnumerable<Slider>> GetAllAsync();
    Task<Slider> GetByIdAsync(int id);
    Task<Slider> AddAsync(CreateSliderDto dto, IFormFile imageFile);
    Task UpdateAsync(UpdateSliderDto dto, IFormFile imageFile);
    Task DeleteAsync(int id);
}