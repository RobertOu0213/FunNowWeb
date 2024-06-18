using Microsoft.AspNetCore.Mvc;
using PrjFunNowWebApi.Models;
using System.Text.Json;

namespace PrjFunNowWeb.Controllers
{
    public class GetMemberController : Controller
    {
        public IActionResult GetMemberId()
        {

            var user = HttpContext.Session.GetString("MemberInfo"); 
            if (string.IsNullOrEmpty(user))
            {
                return Unauthorized(); 
            }

            var userId = JsonSerializer.Deserialize<MemberInfo>(user).MemberId;
            return Ok(new { MemberID = userId });

        }
    }
}
