using Microsoft.AspNetCore.Mvc;
using PrjFunNowWebApi.Models;
using System.Text.Json;

namespace PrjFunNowWeb.Controllers
{
    public class HotelCreateController : Controller
    {
        public IActionResult Hotel()
        {
            return View();
        }

        public IActionResult HotelInfo()
        {
            return View();
        }


        public IActionResult Address()
        {
            return View();
        }

        public IActionResult RoomInfo()
        {
            return View();
        }


        public IActionResult UploadImage()
        {
            //需要將ＭemberId帶到API
            var userID = HttpContext.Session.GetString("MemberID");
            if (string.IsNullOrEmpty(userID))
            {
                userID = HttpContext.Session.GetString("GoogleMemberID");
                if (string.IsNullOrEmpty(userID))
                {
                    return RedirectToAction("Login", "Member");
                }
            }




            ViewBag.UserID = userID;
            return View();
        }
    }
}
