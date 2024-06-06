using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models;
using System.Diagnostics;
using System.Text.Json;

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
            //{
            //    string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER); // 先拿到JSON格式的Session Key

            //    Member x = JsonSerializer.Deserialize<Member>(json); //把JSON字串變回<Member>物件

            //    ViewData["SessionKey"] = "歡迎會員編號為" + x.MemberId + "的" + x.FirstName + x.LastName + "登入~~~";  //包在ViewData裡面，給其他頁面使用

                return View();
            //}
            //return RedirectToAction("Login"); //如果沒拿到session key就回到登入頁面

        }

        public IActionResult Index2()
        {
            return View();
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
