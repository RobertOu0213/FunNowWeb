using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.Models.DTO;
using PrjFunNowWeb.Models.ViewModel;
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
            ViewData["SessionKey"] = 1;
            return View();
        }

        [AuthTokenFilter]
        public IActionResult Index2(string searchValue = null)// 将搜索值放入ViewBag(by louieee)
        {
            var viewModel = new HotelViewModel
            {
                HotelTypes = _context.HotelTypes.ToList(),
                HotelEquipments = _context.HotelEquipments.ToList(),
                Cities = _context.Cities.ToList(),
                RoomEquipments = _context.RoomEquipments.ToList()
            };

            // 将搜索值放入ViewBag(by louieee)
            ViewBag.SearchValue = searchValue;

            return View(viewModel);
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
