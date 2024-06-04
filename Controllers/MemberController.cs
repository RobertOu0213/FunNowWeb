using Microsoft.AspNetCore.Mvc;

namespace PrjFunNowWeb.Controllers
{
    public class MemberController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult MemberInformation()
        {
            return View();
        }

        public IActionResult HostInformation()
        {
            return View();
        }


    }
}
