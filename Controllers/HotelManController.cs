using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrjFunNowWeb.Models;

namespace PrjFunNowWeb.Controllers
{
    public class HotelManController : Controller
    {
        private readonly FunNowContext _context;
        public HotelManController(FunNowContext context)
        {
            _context = context;
        }

        public IActionResult HotelMessenage()
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
            else
            {
                var existingMember = _context.Members
                    .Where(x => x.MemberId == Convert.ToInt32(userID))
                    .FirstOrDefault();

                if (existingMember == null)
                {
                    return NotFound("Member not found");
                }
            }

            var member = _context.Members
                .Where(x => x.MemberId == Convert.ToInt32(userID))
                .Select(x => new { x.MemberId, x.FirstName, x.LastName })
                .FirstOrDefault();

            if (member == null)
            {
                return NotFound("Member not found");
            }

            ViewBag.MemberID = member.MemberId;
            ViewBag.FirstName = member.FirstName;
            ViewBag.LastName = member.LastName;


            return View();

        }
        public IActionResult Hoteldaily()
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
            else
            {
                var existingMember = _context.Members
                    .Where(x => x.MemberId == Convert.ToInt32(userID))
                    .FirstOrDefault();

                if (existingMember == null)
                {
                    return NotFound("Member not found");
                }
            }

            var member = _context.Members
                .Where(x => x.MemberId == Convert.ToInt32(userID))
                .Select(x => new { x.MemberId, x.FirstName, x.LastName })
                .FirstOrDefault();

            if (member == null)
            {
                return NotFound("Member not found");
            }

            ViewBag.MemberID = member.MemberId;
            ViewBag.FirstName = member.FirstName;
            ViewBag.LastName = member.LastName;


            return View();

        }
    }
}
