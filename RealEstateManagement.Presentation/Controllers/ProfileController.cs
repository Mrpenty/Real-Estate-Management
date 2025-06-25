using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace RealEstateManagement.Presentation.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("REMApi");
            // Lấy token từ cookie
            var token = Request.Cookies["accessToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync("api/User/Get-Profile");
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Auth");
            }
            var profileJson = await response.Content.ReadAsStringAsync();
            ViewBag.ProfileJson = profileJson;
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
    }
} 