using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.Models.ViewModel;

namespace PrjFunNowWeb.Controllers
{
    public class PaymentController : Controller
    {
        private readonly FunNowContext _context;
        public PaymentController(FunNowContext context)
        {
            _context = context;
        }
        public IActionResult thankyou(COrderViewModel orderIn)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if(orderIn == null)
                {
                    return BadRequest("OrderDetailsId is required");
                }
         
                var order = new Order
                {
                    MemberId = 1,
                    OrderStatusId = 1,
                    PaymentStatusId = 1,
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
                transaction.Commit();

                var orderDetails = _context.OrderDetails
                .Where(od => orderIn.OrderDetailsID.Contains(od.OrderDetailId))
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
                .ToList();

                var firstOrderDetail = orderDetails.FirstOrDefault();
                if (firstOrderDetail == null)
                {
                    return NotFound("Order details not found");
                }
                var viewModel = new CThankyouViewModel
                {
                    Order = order,
                    OrderDetails = orderDetails.Select(od => new CReservationSummaryViewModel
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
                        RoomImage = GetImageUrl(od.Room?.RoomImages?.FirstOrDefault()?.RoomImage1)   
                  
                    }).ToList()
                };

                return View(viewModel); 
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return RedirectToAction("PaymentPage", "Cart");
            }
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
