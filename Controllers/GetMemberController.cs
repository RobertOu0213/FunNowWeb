using Microsoft.AspNetCore.Mvc;
using PrjFunNowWebApi.Models;
using System.Text.Json;

namespace PrjFunNowWeb.Controllers
{
    public class GetMemberController : Controller
    {
        public IActionResult GetMemberId()
        {

 
            var userID = HttpContext.Session.GetString("MemberID");
            if (string.IsNullOrEmpty(userID))
            {

                userID = HttpContext.Session.GetString("GoogleMemberID");
                if (string.IsNullOrEmpty(userID))
                {
                    return RedirectToAction("Login", "Member");
                }
            }


            return Ok(new { MemberID = userID });

        }
    }
}
