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
            //int memberId = HttpContext.Session.GetInt32("MemberID") ?? 0; // 使用默认值0作为备选
            int memberId = 1; // 假设房东的 MemberID 是 2
            var orders = from order in _context.Orders
                         join detail in _context.OrderDetails on order.OrderId equals detail.OrderId
                         join room in _context.Rooms on detail.RoomId equals room.RoomId
                         join hotel in _context.Hotels on room.HotelId equals hotel.HotelId
                         join orderstatus in _context.OrderStatuses on order.OrderStatusId equals orderstatus.OrderStatusId
                         where room.MemberId == memberId // 假設房東的 MemberID 已知
                         select new OrderViewModel
                         {
                             OrderNumber = order.OrderId,
                             GuestName = order.GuestFirstName + " " + order.GuestLastName,    
                             Nights=(detail.CheckOutDate-detail.CheckInDate).Days,
                             HotelName = hotel.HotelName,
                             CheckInDate = detail.CheckInDate,
                             CheckOutDate = detail.CheckOutDate,
                             Price = order.TotalPrice,
                             Status = orderstatus.OrderStatusName,
                         };               
            var orderList = orders.ToList();  // 将结果存储到一个变量中，便于调试
            if (!orderList.Any())
            {
                Console.WriteLine("No orders found!"); // 或者使用其他日志记录方式
            }

            return View(orderList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
