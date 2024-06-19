using Microsoft.AspNetCore.Mvc;

namespace PrjFunNowWeb.Controllers
{
    public class HostOrderController : Controller
    {
        public IActionResult HostOrderList()
        {
            return View();
        }
    }
}
