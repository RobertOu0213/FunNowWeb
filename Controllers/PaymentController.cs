using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.Models.DTO;
using PrjFunNowWeb.Models.ViewModel;
using PrjFunNowWebApi.Models;
using System.Text.Json;

namespace PrjFunNowWeb.Controllers
{
    public class PaymentController : Controller
    {
        private readonly FunNowContext _context;
        public PaymentController(FunNowContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult thankyou([FromBody] COrderViewModel orderIn)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if(orderIn == null)
                {
                    return BadRequest("OrderDetailsId is required");
                }

                var userID = HttpContext.Session.GetString("MemberID");
                if (string.IsNullOrEmpty(userID))
                {
                    userID = HttpContext.Session.GetString("GoogleMemberID");
                    if (string.IsNullOrEmpty(userID))
                    {
                        return RedirectToAction("Login", "Member");
                    }
                }


                var order = new Order
                {
                    MemberId = Convert.ToInt32(userID),
                    OrderStatusId = 1,    //已付款
                    TotalPrice = orderIn.TotalPrice,
                    CreatedAt = DateTime.Now,
                    GuestLastName = orderIn.GuestLastName,
                    GuestFirstName = orderIn.GuestFirstName,
                    GuestEmail = orderIn.GuestEmail
                };
                _context.Orders.Add(order);
                _context.SaveChanges();

           
                var orderId = order.OrderId;


                foreach (var detailId in orderIn.OrderDetailsID)
                {
                    var orderDetail = _context.OrderDetails.Find(detailId);
                    if (orderDetail != null)
                    {
                        orderDetail.OrderId = orderId;
                        orderDetail.IsOrdered = true;

                    }
                }
                _context.SaveChanges();

                var responseOrder = _context.Orders
                    .Where(x => x.OrderId == orderId)
                    .Include(x => x.OrderDetails)
                        .ThenInclude(od => od.Room)
                            .ThenInclude(r => r.Hotel)
                                .ThenInclude(h => h.HotelImages)
                    .Include(x => x.OrderDetails)
                        .ThenInclude(od => od.Room)
                            .ThenInclude(r => r.RoomType)
                    .Include(x => x.OrderDetails)
                        .ThenInclude(od => od.Room)
                            .ThenInclude(r => r.Hotel)
                                .ThenInclude(h => h.City)
                                    .ThenInclude(c => c.Country)
                    .Include(x => x.OrderDetails)
                        .ThenInclude(od => od.Room)
                            .ThenInclude(r => r.Hotel)
                                .ThenInclude(h => h.HotelType)
                    .Include(x => x.OrderDetails)
                        .ThenInclude(od => od.Member)
                    .ToList();


                var orderDto = responseOrder.SelectMany(o => o.OrderDetails.Select(od => new COrderSuccessEmailDTO
                {
                    CheckInDate = od.CheckInDate,
                    CheckOutDate = od.CheckOutDate,
                    NumberOfDays = (od.CheckOutDate - od.CheckInDate).Days,
                    RoomName = od.Room.RoomName,
                    RoomPrice = od.Room.RoomPrice,
                    RoomType = od.Room.RoomType.RoomTypeName,
                    GuestNumber = od.GuestNumber,
                    HotelName = od.Room.Hotel.HotelName,
                    HotelAddress = od.Room.Hotel.HotelAddress,
                    HotelPhone = od.Room.Hotel.HotelPhone,
                    LevelStar = od.Room.Hotel.LevelStar,
                    CityName = od.Room.Hotel.City.CityName,
                    CountryName = od.Room.Hotel.City.Country.CountryName,
                    HotelImage = GetImageUrl(od.Room.Hotel.HotelImages.FirstOrDefault()?.HotelImage1),
                    GuestFirstName = o.GuestFirstName,
                    GuestLastName = o.GuestLastName,
                    Email = od.Member.Email,
                    TotalPrice = o.TotalPrice,
                    EachRoomTotalPrice = od.Room.RoomPrice * ((od.CheckOutDate - od.CheckInDate).Days)
                })).ToList();


                transaction.Commit();

                return Ok(new { success = true, message="Order 資料表新增成功", data= orderDto });

                //var orderDetails = _context.OrderDetails
                //.Where(od => orderIn.OrderDetailsID.Contains(od.OrderDetailId))
                //.Include(od => od.Room)
                //    .ThenInclude(r => r.Hotel)
                //        .ThenInclude(h => h.City)
                //.Include(od => od.Room)
                //    .ThenInclude(r => r.RoomType)
                //.Include(od => od.Room)
                //    .ThenInclude(r => r.Hotel)
                //        .ThenInclude(h => h.Comments)
                //.Include(od => od.Room)
                //    .ThenInclude(r => r.RoomImages)
                //.Include(od => od.Member)
                //.ToList();

                //var firstOrderDetail = orderDetails.FirstOrDefault();
                //if (firstOrderDetail == null)
                //{
                //    return NotFound("Order details not found");
                //}
                //var viewModel = new CThankyouViewModel
                //{
                //    Order = order,
                //    OrderDetails = orderDetails.Select(od => new CReservationSummaryViewModel
                //    {
                //        OrderDetailID = od.OrderDetailId,
                //        HotelName = od.Room?.Hotel?.HotelName,
                //        RoomType = od.Room?.RoomType?.RoomTypeName,
                //        RoomName = od.Room?.RoomName,
                //        RoomPrice = od.Room?.RoomPrice ?? 0,
                //        CityName = od.Room?.Hotel?.City?.CityName,
                //        AllCommentsCount = od.Room?.Hotel?.Comments?.Count() ?? 0,
                //        LevelStar = od.Room?.Hotel?.LevelStar ?? 0,
                //        CheckInDate = od.CheckInDate,
                //        CheckOutDate = od.CheckOutDate,
                //        RoomID = od.RoomId,
                //        MaximumOccupancy = od.Room?.MaximumOccupancy ?? 0,
                //        AllOrderDetailsCount = orderDetails.Count,
                //        RoomImage = GetImageUrl(od.Room?.RoomImages?.FirstOrDefault()?.RoomImage1),
                //        HotelID= (int)od.Room?.Hotel?.HotelId
                //    }).ToList()
                //};

                //return View(viewModel); 
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                //return RedirectToAction("PaymentPage", "Cart");
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }


        public IActionResult thankyou2()
        {
            return View();
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
