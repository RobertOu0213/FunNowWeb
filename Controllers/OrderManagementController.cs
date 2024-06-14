using Microsoft.AspNetCore.Mvc;

namespace PrjFunNowWeb.Controllers
{
    public class OrderManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrderCallApi()
        {
            return View();
        }
        public IActionResult OrderDetail()
        {
            return View();
        }
    }
}
