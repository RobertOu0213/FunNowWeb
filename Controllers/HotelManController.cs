using Microsoft.AspNetCore.Mvc;

namespace PrjFunNowWeb.Controllers
{
    public class HotelManController : Controller
    {
        public IActionResult HotelMessenage()
        {
            return View();
        }
    }
}
