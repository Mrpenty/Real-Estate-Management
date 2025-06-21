using Microsoft.AspNetCore.Mvc;

namespace RealEstateManagement.Presentation.Controllers
{
    public class PostPropertyController : Controller
    {
        public IActionResult CreateProperty()
        {
            return View();
        }

        public IActionResult CreateContract(int propertyPostId)
        {
            ViewBag.PropertyPostId = propertyPostId;
            return View();
        }
    }
} 