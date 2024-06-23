using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrjFunNowWeb.Models;

namespace PrjFunNowWeb.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Chatroom()
        {
            return View();
        }
        private readonly FunNowContext _context;

        public ChatController(FunNowContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMemberIdByHotelId(int hotelId)
        {
            // 添加調試輸出
            Console.WriteLine($"Received hotelId: {hotelId}");

            var hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == hotelId);
            if (hotel == null)
            {
                Console.WriteLine("Hotel not found");
                return NotFound(new { Message = "Hotel not found" });
            }

            var memberId = hotel.MemberId;
            Console.WriteLine($"Found memberId: {memberId}");
            return Ok(new { MemberId = memberId });
        }
        public IActionResult Mailroom()
        {
            var memberId = HttpContext.Session.GetString("MemberID");
            var googleMemberId = HttpContext.Session.GetString("GoogleMemberID");

            if (string.IsNullOrEmpty(memberId) && string.IsNullOrEmpty(googleMemberId))
            {
                return RedirectToAction("Login", "Member");
            }

            ViewBag.MemberID = !string.IsNullOrEmpty(googleMemberId) ? googleMemberId : memberId;
            return View();
        }
        public IActionResult CSCustomerService()
        {
            return View();
        }
        public IActionResult guest()
        {
            return View();
        }
    }
}
