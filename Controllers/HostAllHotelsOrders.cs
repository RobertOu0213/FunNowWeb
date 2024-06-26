﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.ViewModels;
using System.Net.Http;

namespace PrjFunNowWeb.Controllers
{
    public class HostAllHotelsOrders : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FunNowContext _context;
        private readonly HttpClient _httpClient;


        public HostAllHotelsOrders(ILogger<HomeController> logger, FunNowContext context, HttpClient httpClient)
        {
            _logger = logger;
            _context = context;
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HostHotelsOdersList()
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


            int memberIdValue = int.Parse(userID);

            // 使用 memberId 进行查询
            var orders = from order in _context.Orders                 //LINQ語法  from...in....
                         join detail in _context.OrderDetails on order.OrderId equals detail.OrderId
                         join room in _context.Rooms on detail.RoomId equals room.RoomId
                         join hotel in _context.Hotels on room.HotelId equals hotel.HotelId
                         join orderstatus in _context.OrderStatuses on order.OrderStatusId equals orderstatus.OrderStatusId
                         where room.MemberId == memberIdValue // 假設房東的 MemberID 已知
                         select new OrderViewModel
                         {
                             MemberId = memberIdValue,
                             OrderNumber = order.OrderId,
                             GuestName = order.GuestFirstName + " " + order.GuestLastName,
                             Nights = (detail.CheckOutDate - detail.CheckInDate).Days,
                             HotelName = hotel.HotelName,
                             CheckInDate = detail.CheckInDate,
                             CheckOutDate = detail.CheckOutDate,
                             Price = order.TotalPrice,
                             Status = orderstatus.OrderStatusName,
                         };

            return View(orders.ToList());
        }
    }
}
