using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.Models.DTO;
using System.Text.Json;
namespace PrjFunNowWeb.Controllers
{
    public class Cart : Controller
    {
        private readonly FunNowContext _context;
        public Cart(FunNowContext context)
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

        [HttpPost]
        public IActionResult PreparePayment([FromBody] List<int> orderDetailsId)
        {
            if (orderDetailsId == null || !orderDetailsId.Any())
            {
                return BadRequest("OrderDetailsId is required");
            }

   
            TempData["OrderDetailsId"] = JsonSerializer.Serialize(orderDetailsId);

            var redirectUrl = Url.Action("PaymentPage", "Cart");
            return Json(new { success = true, redirectUrl });
        }


        public IActionResult PaymentPage()
        {
           
            var orderDetailsIdJson = TempData["OrderDetailsId"] as string;

            if (string.IsNullOrEmpty(orderDetailsIdJson))
            {
                return BadRequest("OrderDetailsId is required");
            }

            var orderDetailsId = JsonSerializer.Deserialize<List<int>>(orderDetailsIdJson);
            ViewBag.OrderDetailsId = orderDetailsId;
            return View();
        }


    }
}
