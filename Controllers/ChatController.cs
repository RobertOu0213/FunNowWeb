using Microsoft.AspNetCore.Mvc;

namespace PrjFunNowWeb.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Chatroom()
        {
            return View();
        }
        public IActionResult Mailroom()
        {
            return View();
        }
        public IActionResult CSCustomerService()
        {
            return View();
        }
        public IActionResult guest()
        {
            return View();
        }
    }
}
