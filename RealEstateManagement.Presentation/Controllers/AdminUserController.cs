using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateManagement.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        // GET: /AdminUser
        public IActionResult Index()
        {
            ViewBag.Title = "Quản lý người dùng";
            return View();
        }
    }
} 