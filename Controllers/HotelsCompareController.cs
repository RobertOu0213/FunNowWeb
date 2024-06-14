using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models;

namespace PrjFunNowWeb.Controllers
{
    public class HotelsCompareController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly FunNowContext _context;

        public HotelsCompareController(ILogger<HomeController> logger, FunNowContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Hotelstest()
        {
            return View();
        }


        public IActionResult HotelsCompareAccordion()
        {
            return View();
        }
    }
}
