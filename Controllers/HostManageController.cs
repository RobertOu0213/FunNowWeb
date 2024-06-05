using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.ViewModels;

namespace PrjFunNowWeb.Controllers
{
    public class HostManageController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FunNowContext _context;

        public HostManageController(ILogger<HomeController> logger, FunNowContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult HostHotelsOdersList()
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
                             MemberId = memberId,
                             OrderNumber = order.OrderId,
                             GuestName = order.GuestFirstName + " " + order.GuestLastName,
                             Nights = (detail.CheckOutDate - detail.CheckInDate).Days,
                             HotelName = hotel.HotelName,
                             CheckInDate = detail.CheckInDate,
                             CheckOutDate = detail.CheckOutDate,
                             Price = order.TotalPrice,
                             Status = orderstatus.OrderStatusName,
                         };

            return View(orders.ToList());
        }
    }
}
