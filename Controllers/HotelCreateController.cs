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
            var user = HttpContext.Session.GetString("MemberInfo");
            if (string.IsNullOrEmpty(user))
            {

                return RedirectToAction("Login", "Member");
            }
            var userId = JsonSerializer.Deserialize<MemberInfo>(user).MemberId;

            return View(userId);
        }
    }
}
