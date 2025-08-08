using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly RentalDbContext _context;

        public OpenAIService(IConfiguration configuration, RentalDbContext rentalDbContext)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"]; // Lấy từ appsettings.json
            _context = rentalDbContext;
        }

        public async Task<string> AskGPTAsync(RealEstateDescriptionRequest request)
        {
            string prompt = await BuildPromptFromRequestAsync(request);

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new object[]
                {
                    new { role = "system", content = "Bạn dựa vào thông tin tôi cung cấp viết Description lịch sự." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.7
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OpenAI API error: {(int)response.StatusCode} - {responseString}");
            }

            dynamic result = JsonConvert.DeserializeObject(responseString);
            return result.choices[0].message.content.ToString();
        }

        private async Task<string> BuildPromptFromRequestAsync(RealEstateDescriptionRequest req)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Viết mô tả hấp dẫn cho bất động sản cho thuê.");
            builder.AppendLine($"Tên Title: {req.Title}");
            builder.AppendLine($"Loại nhà: {req.Type}");
            builder.AppendLine($"Diện tích: {req.Area} m2");
            builder.AppendLine($"Số phòng ngủ: {req.Bedrooms}");
            builder.AppendLine($"Giá thuê: {req.Price} triệu đồng/tháng (Có thể thay đổi theo thỏa thuận)");
            var amenityDescriptions = await GetAmenityDescriptionsByIdsAsync(req.AmenityIds);
            if (amenityDescriptions.Any())
                builder.AppendLine($"Tiện ích bao gồm: {string.Join(", ", amenityDescriptions)}");
            builder.AppendLine($"Tiện ích: {string.Join(", ", amenityDescriptions)}");
            builder.AppendLine($"Địa chỉ: {req.DetailedAddress}, Đường (StreetID): {req.StreetId}, Phường (WardID): {req.WardId}, Tỉnh (ProvinceID): {req.ProvinceId}");
            builder.AppendLine($"Viết bằng tiếng Việt, giọng thân thiện, chuyên nghiệp, chi tiết 1 chút.");
            return builder.ToString();
        }

        private async Task<List<string>> GetAmenityDescriptionsByIdsAsync(List<int> amenityIds)
        {
            if (amenityIds == null || !amenityIds.Any())
                return new List<string>();

            return await _context.Amenities
                .Where(a => amenityIds.Contains(a.Id))
                .Select(a => a.Description)
                .ToListAsync();
        }

    }
}
