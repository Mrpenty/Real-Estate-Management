using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateManagement.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: /Admin
        public IActionResult Index()
        {
            ViewBag.Title = "Quản lý người dùng";
            return View();
        }

        // GET: /Admin/PostManagement
        public IActionResult PostManagement()
        {
            ViewBag.Title = "Quản lý bài đăng";
            return View();
        }

        // GET: /Admin/PostDetail/{id}
        public IActionResult PostDetail(int id)
        {
            ViewBag.Title = "Chi tiết bài đăng";
            ViewBag.PostId = id;
            return View();
        }

        // GET: /Admin/PackageManagement
        public IActionResult PackageManagement()
        {
            ViewBag.Title = "Quản lý gói khuyến mãi";
            return View();
        }

        // GET: /Admin/NotificationManagement
        public IActionResult NotificationManagement()
        {
            ViewBag.Title = "Quản lý thông báo";
            return View();
        }
    }
} 