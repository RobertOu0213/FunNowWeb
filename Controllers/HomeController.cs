using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models;
using System.Diagnostics;

namespace PrjFunNowWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FunNowContext _context;

        public HomeController(ILogger<HomeController> logger, FunNowContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult test()
        {
          
           return View(_context.Countries);
        }

        public IActionResult Index()
        {
            //if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER)) //如果有拿到session key
            return View();

            //return RedirectToAction("Login"); //如果沒拿到session key就回到登入頁面
            
        }
        public IActionResult Index2()
        {
            //if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER)) //如果有拿到session key
                return View();

            //return RedirectToAction("Login"); //如果沒拿到session key就回到登入頁面
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
