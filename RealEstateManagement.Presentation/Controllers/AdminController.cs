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

        /// <summary>
        /// Admin Dashboard - Thống kê tổng quan
        /// </summary>
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult NewsManagement()
        {
            ViewBag.Title = "Quản lý bài báo";
            return View();
        }

        [HttpGet]
        public IActionResult NewsDetail(int id)
        {
            ViewBag.Title = "Chi tiết bài báo";
            ViewBag.NewsId = id;
            return View();
        }

        [HttpGet]
        public IActionResult CreateOrEditNews(int? id)
        {
            ViewBag.NewsId = id;
            return View();
        }


        public IActionResult SliderManagement()
        {
            ViewBag.Title = "Quản lý slider";
            return View();
        }

        public IActionResult AmenityManagement()
        {
            ViewBag.Title = "Quản lý tiện ích";
            return View();
        }

        public IActionResult ReportManagement()
        {
            ViewBag.Title = "Quản lý báo cáo";
            return View();
        }
    }
} 