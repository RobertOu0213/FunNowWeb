using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.ViewModels;
using System.Net.Http;

namespace PrjFunNowWeb.Controllers
{
    public class HostAllHotelsOrders : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FunNowContext _context;
        private readonly HttpClient _httpClient;


        public HostAllHotelsOrders(ILogger<HomeController> logger, FunNowContext context, HttpClient httpClient)
        {
            _logger = logger;
            _context = context;
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HostHotelsOdersList()
        {

            // int memberId = (int)Client.Session.GetString("MemberID");         // TODO....  使用默认值0作为备选 等之後登入好了
            //int memberId = 1;
            var memberId = HttpContext.Session.GetString("MemberId");
            if (string.IsNullOrEmpty(memberId))
            {
                // 處理 MemberId 為 null 或空字符串的情況
                return RedirectToAction("Login", "Account"); // 重定向到登錄頁面
            }

            int memberIdValue = int.Parse(memberId);

            // 使用 memberId 进行查询
            var orders = from order in _context.Orders                 //LINQ語法  from...in....
                         join detail in _context.OrderDetails on order.OrderId equals detail.OrderId
                         join room in _context.Rooms on detail.RoomId equals room.RoomId
                         join hotel in _context.Hotels on room.HotelId equals hotel.HotelId
                         join orderstatus in _context.OrderStatuses on order.OrderStatusId equals orderstatus.OrderStatusId
                         where room.MemberId == memberIdValue // 假設房東的 MemberID 已知
                         select new OrderViewModel
                         {
                             MemberId = memberIdValue,
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
