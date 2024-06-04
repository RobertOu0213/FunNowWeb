using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.ViewModels;
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult OwnerHotelsOdersList()
        {
            //var orders = from order in _context.Orders
            //             join detail in _context.OrderDetails on order.OrderID equals detail.OrderID
            //             join room in _context.Rooms on detail.RoomID equals room.RoomID
            //             join hotel in _context.Hotels on room.HotelID equals hotel.HotelID
            //             where room.MemberID == YOUR_MEMBER_ID // 假設房東的 MemberID 已知
            //             select new OrderViewModel
            //             {
            //                 GuestInfo = order.GuestFirstName + " " + order.GuestLastName,
            //                 HotelName = hotel.HotelName,
            //                 CheckInDate = detail.CheckInDate,
            //                 CheckOutDate = detail.CheckOutDate,
            //                 Price = order.TotalPrice,
            //                 Status = (order.OrderStatusID == SOME_STATUS_ID) ? "已確認" : "未確認" // 假設狀態ID代表特定狀態
            //             };

            return View(/*orders.ToList()*/);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
