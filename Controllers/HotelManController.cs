using Microsoft.AspNetCore.Mvc;

namespace PrjFunNowWeb.Controllers
{
    public class HotelManController : Controller
    {
        public IActionResult HotelMessenage()
        {
            // 从Session中获取MemberID和GoogleMemberID
            var memberID = HttpContext.Session.GetString("MemberID");
            var googleMemberId = HttpContext.Session.GetString("GoogleMemberID");

            // 将值传递到视图
            ViewBag.MemberID = !string.IsNullOrEmpty(googleMemberId) ? googleMemberId : memberID;

            return View();
        }
    }
}
