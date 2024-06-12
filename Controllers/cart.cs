using Microsoft.AspNetCore.Mvc;

namespace PrjFunNowWeb.Controllers
{
    public class cart : Controller
    {
        public IActionResult cartItems()
        {
            return View();
        }
    }
}
