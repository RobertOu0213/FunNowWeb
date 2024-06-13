using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models;

namespace PrjFunNowWeb.Controllers
{
    public class cart : Controller
    {
        private readonly FunNowContext _context;
        public cart(FunNowContext context)
        {
            _context = context;
        }
        public IActionResult cartItems(int? userId)
        {
            if (userId <= 0)
            {
                return BadRequest("UserID is required");
            }

            var MemberID = _context.Members.Where(x => x.MemberId == userId).Select(x => x.MemberId).FirstOrDefault();

            if (MemberID <= 0)
            {
                return NotFound("Member not found");
            }

            return View(MemberID);

        }
    }
}
