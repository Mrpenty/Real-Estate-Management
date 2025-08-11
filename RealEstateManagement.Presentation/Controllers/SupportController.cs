using Microsoft.AspNetCore.Mvc;

namespace RealEstateManagement.Presentation.Controllers
{
    public class SupportController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Hỗ trợ khách hàng";
            return View();
        }
    }
} 