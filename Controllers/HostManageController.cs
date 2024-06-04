using Microsoft.AspNetCore.Mvc;

namespace PrjFunNowWeb.Controllers
{
    public class HostManageController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
