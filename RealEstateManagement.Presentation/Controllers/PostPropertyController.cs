using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RealEstateManagement.Presentation.Controllers
{
    //[Authorize]
    public class PostPropertyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PostPropertyController> _logger;

        public PostPropertyController(IHttpClientFactory httpClientFactory, ILogger<PostPropertyController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult CreateProperty()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateProperty1()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProperty([FromBody] JsonElement model)
        {
            try
            {
                //comment
                var token = HttpContext.Request.Cookies["accessToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, message = "User not authenticated." });
                }

                var client = _httpClientFactory.CreateClient("REMApi");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var jsonString = model.ToString();
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                _logger.LogInformation("Forwarding request to API with token: {Token}, Payload: {Payload}", token, jsonString);

                var response = await client.PostAsync("api/PropertyPosts", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("API Success Response: {Response}", responseString);
                    var createdPost = JsonSerializer.Deserialize<JsonElement>(responseString);
                    var propertyPostId = createdPost.GetProperty("id").GetInt32();
                    return Json(new { success = true, propertyPostId });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API Error Response: {StatusCode} - {Response}", response.StatusCode, errorContent);
                    return Json(new { success = false, message = $"API Error: {errorContent}" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateProperty POST action.");
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Compare([FromBody] JsonElement model)
        {
            try
            {

                var client = _httpClientFactory.CreateClient("REMApi");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var jsonString = model.ToString();
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                _logger.LogInformation("Payload: {Payload}", jsonString);

                var response = await client.PostAsync("api/Property/Compare", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("API Success Response: {Response}", responseString);
                    return Json(new { success = true, responseString });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API Error Response: {StatusCode} - {Response}", response.StatusCode, errorContent);
                    return Json(new { success = false, message = $"API Error: {errorContent}" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateProperty POST action.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ListProperty()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DetailProperty(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        public IActionResult CreateContract(int propertyPostId)
        {
            ViewBag.PropertyPostId = propertyPostId;
            return View();
        }
    }
} 