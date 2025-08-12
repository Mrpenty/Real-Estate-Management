using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Presentation.Models;
using System.Diagnostics;

namespace RealEstateManagement.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //ViewBag.ProvinceId = province;
            //ViewBag.WardId = ward;
            //ViewBag.StreetId = street;
            //ViewBag
            return View();
        }

        public IActionResult NewDetail(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public IActionResult Landlord(int id)
        {
            ViewBag.LandlordId = id;
            return View();
        }

        public IActionResult News()
        {
            return View();
        }
        public IActionResult Detail(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult ListFavorite()
        {
            return View();
        }

        public IActionResult ListInterest()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}