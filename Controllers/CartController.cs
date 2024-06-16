using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.Models.DTO;
using PrjFunNowWeb.Models.ViewModel;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Security.Cryptography;
namespace PrjFunNowWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly FunNowContext _context;
        public CartController(FunNowContext context)
        {
            _context = context;
        }

        //[Route("cart/cartItems/{userId}")]
        public IActionResult cartItems(int? id)
        {
            //登入controller 判斷




            if (id <= 0 || id == null)
            {
                return BadRequest("UserID is required");
            }

            var MemberID = _context.Members.Where(x => x.MemberId == id).Select(x => x.MemberId).FirstOrDefault();

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


        public async Task<IActionResult> PaymentPage()
        {
            try
            {
                var orderDetailsIdJson = TempData["OrderDetailsId"] as string;

                if (string.IsNullOrEmpty(orderDetailsIdJson))
                {
                    return BadRequest("OrderDetailsId is required");
                }

                var orderDetailsId = JsonSerializer.Deserialize<List<int>>(orderDetailsIdJson);

                var orderDetails = await _context.OrderDetails
                    .Where(od => orderDetailsId.Contains(od.OrderDetailId))
                    .Include(od => od.Room)
                        .ThenInclude(r => r.Hotel)
                            .ThenInclude(h => h.City)
                    .Include(od => od.Room)
                        .ThenInclude(r => r.RoomType)
                    .Include(od => od.Room)
                        .ThenInclude(r => r.Hotel)
                            .ThenInclude(h => h.Comments)
                    .Include(od => od.Room)
                        .ThenInclude(r => r.RoomImages)
                    .Include(od => od.Member)
                    .ToListAsync();

                var firstOrderDetail = orderDetails.FirstOrDefault();
                if (firstOrderDetail == null)
                {
                    return NotFound("Order details not found");
                }

                var totalAmount = orderDetails.Sum(od =>Convert.ToInt32((od.Room?.RoomPrice ?? 0) * (od.CheckOutDate - od.CheckInDate).Days));


                //綠界金流
                var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
                var website = $"https://localhost:7284/";
                var ecpayParameters = new Dictionary<string, string>
                     {
                         { "MerchantTradeNo",  orderId },
                         { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") },
                         { "TotalAmount",  totalAmount.ToString() }, 
                         { "TradeDesc",  "無" },
                         { "ItemName",  "測試商品" },
                         { "ExpireDate",  "3" },
                         { "CustomField1",  "" },
                         { "CustomField2",  "" },
                         { "ReturnURL",  $"{website}/Payment/thankyou" },
                         //{ "OrderResultURL", $"{website}/Home/PayInfo/{orderId}" },
                         //{ "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo" },
                         //{ "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}" },
                         { "MerchantID",  "3002607" },
                         //{ "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE" },
                         { "PaymentType",  "aio" },
                         { "ChoosePayment",  "ALL" },
                         { "EncryptType",  "1" },
                     };

                ecpayParameters["CheckMacValue"] = GetCheckMacValue(ecpayParameters);





                var member = firstOrderDetail.Member;
                var viewModel = orderDetails.Select(od => new CReservationSummaryViewModel
                {
                    OrderDetailID = od.OrderDetailId,
                    HotelName = od.Room?.Hotel?.HotelName,
                    RoomType = od.Room?.RoomType?.RoomTypeName,
                    RoomName = od.Room?.RoomName,
                    RoomPrice = od.Room?.RoomPrice ?? 0,
                    CityName = od.Room?.Hotel?.City?.CityName,
                    AllCommentsCount = od.Room?.Hotel?.Comments?.Count() ?? 0,
                    LevelStar = od.Room?.Hotel?.LevelStar ?? 0,
                    CheckInDate = od.CheckInDate,
                    CheckOutDate = od.CheckOutDate,
                    RoomID = od.RoomId,
                    MaximumOccupancy = od.Room?.MaximumOccupancy ?? 0,
                    AllOrderDetailsCount = orderDetails.Count,
                    RoomImage = GetImageUrl(od.Room?.RoomImages?.FirstOrDefault()?.RoomImage1),
                    MemberID = member?.MemberId ?? 0,
                    FirstName = member?.FirstName,
                    LastName = member?.LastName,
                    Email = member?.Email,
                    Phone = member?.Phone,
                    HotelID = (int)od.Room?.Hotel?.HotelId,
                    EcpayParameters = ecpayParameters
                }).ToList();

                ViewBag.Member = member;

                return View(viewModel);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }

        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);
            var hashKey = "pwFHCqoQZGmho4w6";
            var HashIV = "EkRm7iFT261dpevs";
            checkValue = $"HashKey={hashKey}&{checkValue}&HashIV={HashIV}";
            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }

        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256Managed.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        private string GetImageUrl(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return string.Empty;
            }

            if (imagePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                return imagePath;
            }
            else
            {
                return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/image/{imagePath}";
            }
        }



    }
}
