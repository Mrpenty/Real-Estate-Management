using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.DTO.Location;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RealEstateManagement.Business.Repositories.AddressRepo;

namespace RealEstateManagement.Business.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly RentalDbContext _context;
        private readonly IAddressRepository _addressRepository;

        public OpenAIService(RentalDbContext rentalDbContext, IAddressRepository addressRepository)
        {
            _httpClient = new HttpClient();
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
              ?? throw new Exception("OPENAI_API_KEY not found in environment");
            _context = rentalDbContext;
            _addressRepository = addressRepository;
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

        public async Task<string> GetPropertyRecommendationAsync(LocationRecommendationRequest request, List<PropertyRecommendationDTO> properties)
        {
            try
            {
                string prompt = BuildRecommendationPrompt(request, properties);

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new object[]
                    {
                        new { role = "system", content = "Bạn là chuyên gia tư vấn bất động sản. Hãy phân tích và đưa ra lý do tại sao những bất động sản này phù hợp với người dùng dựa trên vị trí và tiêu chí của họ." },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.7,
                    max_tokens = 300
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
            catch (Exception ex)
            {
                return "Dựa trên vị trí và tiêu chí tìm kiếm của bạn, chúng tôi đã tìm thấy những bất động sản phù hợp nhất.";
            }
        }

        private string BuildRecommendationPrompt(LocationRecommendationRequest request, List<PropertyRecommendationDTO> properties)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Người dùng đang tìm kiếm bất động sản tại vị trí: {request.Latitude}, {request.Longitude}");
            builder.AppendLine($"Bán kính tìm kiếm: {request.RadiusKm}km");
            
            if (!string.IsNullOrEmpty(request.PropertyType))
                builder.AppendLine($"Loại bất động sản: {request.PropertyType}");
            
            if (request.MaxPrice.HasValue)
                builder.AppendLine($"Giá tối đa: {request.MaxPrice} triệu đồng/tháng");
            
            if (request.MinBedrooms.HasValue)
                builder.AppendLine($"Số phòng ngủ tối thiểu: {request.MinBedrooms}");
            
            if (!string.IsNullOrEmpty(request.UserPreference))
                builder.AppendLine($"Sở thích: {request.UserPreference}");
            
            builder.AppendLine($"\nĐã tìm thấy {properties.Count} bất động sản phù hợp:");
            
            foreach (var prop in properties.Take(5)) // Chỉ lấy 5 bất động sản đầu tiên để phân tích
            {
                builder.AppendLine($"- {prop.Title}: {prop.Type}, {prop.Bedrooms} phòng ngủ, {prop.Area}m², {prop.Price} triệu/tháng, cách {prop.DistanceKm:F1}km");
            }
            
            builder.AppendLine("\nHãy đưa ra lý do tại sao những bất động sản này phù hợp với người dùng, dựa trên vị trí, tiêu chí và sở thích của họ.");
            
            return builder.ToString();
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

            var address = await _addressRepository.GetAddressAsync(req.ProvinceId ?? 0, req.WardId ?? 0, req.StreetId ?? 0, req.DetailedAddress ?? String.Empty);
            if (address != null)
            {
                builder.AppendLine(
                    $"Địa chỉ: {req.DetailedAddress}, " +
                    $"Đường: {address.Street?.Name}, " +
                    $"Phường: {address.Ward?.Name}, " +
                    $"Tỉnh: {address.Province?.Name}"
                );
            }
            else
            {
                builder.AppendLine($"Địa chỉ: {req.DetailedAddress}");
            }
            builder.AppendLine($"Viết bằng tiếng Việt, giọng thân thiện, chuyên nghiệp, chi tiết 1 chút. Thêm cả những địa điểm nổi tiếng gần địa chỉ ở phía dưới nữa");
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
