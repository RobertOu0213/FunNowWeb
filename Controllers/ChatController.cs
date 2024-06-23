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
