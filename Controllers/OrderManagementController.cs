using Microsoft.AspNetCore.Mvc;

namespace PrjFunNowWeb.Controllers
{
    public class OrderManagementController : Controller
    {
        public IActionResult Index()
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

        public IActionResult OrderCallApi()
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

        public IActionResult OrderDetail()
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

        public IActionResult HotelMessenge()
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
        public IActionResult Customerservice()
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
        public IActionResult CustomerbeautyService()
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
    }
}