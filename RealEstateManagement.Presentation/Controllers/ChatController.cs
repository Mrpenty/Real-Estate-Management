using Microsoft.AspNetCore.Mvc;

namespace RealEstateManagement.Presentation.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index(int conversationId)
        {
            ViewBag.ConversationId = conversationId;
            return View();
        }
    }
}
