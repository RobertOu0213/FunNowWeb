using Microsoft.AspNetCore.Mvc;

namespace PrjFunNowWeb.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
