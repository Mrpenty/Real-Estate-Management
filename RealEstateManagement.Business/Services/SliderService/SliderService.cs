using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Nest;
using RealEstateManagement.Business.DTO.SliderDTO;
using RealEstateManagement.Business.Services.UploadPicService;
using RealEstateManagement.Data.Entity;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

public class SliderService : ISliderService
{
    private readonly ISliderRepository _sliderRepository;
    private readonly IUploadPicService _uploadPicService;

    public SliderService(ISliderRepository sliderRepository, IUploadPicService uploadPicService)
    {
        _sliderRepository = sliderRepository;
        _uploadPicService = uploadPicService;
    }

    public async Task<IEnumerable<Slider>> GetAllAsync()
    {
        var list = new List<Slider>()
        {
            new Slider
                {
                    Id = 1,
                    ImageUrl = "https://localhost:7160/image/slider1.jpg",
                    Title = "slider1",
                    Description = "slider1",
                    CreatedAt = DateTime.Now
                },
                new Slider
                {
                    Id = 2,
                    ImageUrl = "https://localhost:7160/image/slider2.jpg",
                    Title = "slider2",
                    Description = "slider2",
                    CreatedAt = DateTime.Now
                },
                new Slider
                {
                    Id = 3,
                    ImageUrl = "https://localhost:7160/image/slider3.jpg",
                    Title = "slider3",
                    Description = "slider3",
                    CreatedAt = DateTime.Now
                }
        };
        //return await _sliderRepository.GetAsync() ?? list;
        return list;
    }

    public async Task<Slider> GetByIdAsync(int id)
    {
        var slider = await _sliderRepository.GetByIdAsync(id);
        if (slider == null) throw new KeyNotFoundException($"Slider with ID {id} not found.");
        return slider;
    }

    private async Task<string> UploadSliderImageAsync(IFormFile imageFile, string title)
    {
        if (imageFile == null || !_uploadPicService.IsValidImageFile(imageFile))
            return null;

        var uploadResult = await _uploadPicService.UploadImageAsync(
            imageFile,
            "slider-pictures",
            $"slider_{title}_{DateTime.UtcNow.Ticks}"
        );
        return uploadResult?.Succeeded == true ? uploadResult.ImageUrl : null;
    }

    public async Task<Slider> AddAsync(CreateSliderDto dto, IFormFile imageFile)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var slider = new Slider
        {
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        var imageUrl = await UploadSliderImageAsync(imageFile, dto.Title);
        if (!string.IsNullOrEmpty(imageUrl))
        {
            slider.ImageUrl = imageUrl;
        }

        await _sliderRepository.AddAsync(slider);
        return slider;
    }

    public async Task UpdateAsync(UpdateSliderDto dto, IFormFile imageFile)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var existingSlider = await _sliderRepository.GetByIdAsync(dto.Id);
        if (existingSlider == null) throw new KeyNotFoundException($"Slider with ID {dto.Id} not found.");

        existingSlider.Title = dto.Title;
        existingSlider.Description = dto.Description;

        if (imageFile != null && _uploadPicService.IsValidImageFile(imageFile))
        {
            if (!string.IsNullOrEmpty(existingSlider.ImageUrl))
            {
                await _uploadPicService.DeleteImageAsync(existingSlider.ImageUrl);
            }
            var newImageUrl = await UploadSliderImageAsync(imageFile, dto.Title);
            if (!string.IsNullOrEmpty(newImageUrl))
            {
                existingSlider.ImageUrl = newImageUrl;
            }
        }

        await _sliderRepository.UpdateAsync(existingSlider);
    }

    public async Task DeleteAsync(int id)
    {
        var slider = await _sliderRepository.GetByIdAsync(id);
        if (slider == null) throw new KeyNotFoundException($"Slider with ID {id} not found.");

        if (!string.IsNullOrEmpty(slider.ImageUrl))
        {
            await _uploadPicService.DeleteImageAsync(slider.ImageUrl);
        }

        await _sliderRepository.DeleteAsync(id);
    }
}