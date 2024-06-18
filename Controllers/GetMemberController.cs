using Microsoft.AspNetCore.Mvc;
using PrjFunNowWebApi.Models;
using System.Text.Json;

namespace PrjFunNowWeb.Controllers
{
    public class GetMemberController : Controller
    {
        public IActionResult GetMemberId()
        {

            var userId = HttpContext.Session.GetString("MemberID"); 
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); 
            }

       
            return Ok(new { MemberID = userId });

        }
    }
}
