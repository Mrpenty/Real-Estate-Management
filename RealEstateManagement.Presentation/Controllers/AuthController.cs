using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RealEstateManagement.Presentation.Controllers
{
    public class AuthController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            // Check if user is already authenticated
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            } 
            return View();
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            // Check if user is already authenticated
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult VerifyOTP(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return RedirectToAction("Register");
            }
            return View();
        }

        public IActionResult Logout()
        {
            // The actual logout is handled by the API through the authService.js
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            // This action will serve the VerifyEmail.cshtml view
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return RedirectToAction("ForgotPassword");
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult TestForgotPassword()
        {
            return View();
        }

    }

   
} 