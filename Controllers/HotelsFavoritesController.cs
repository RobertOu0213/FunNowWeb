﻿using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models;

namespace PrjFunNowWeb.Controllers
{
    public class HotelsFavoritesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FunNowContext _context;

        public HotelsFavoritesController(ILogger<HomeController> logger, FunNowContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NoHotelsFavorite()
        {

            return View();
        }

        public IActionResult HotelsFavoritesList(int memberId)            //抓到會員收藏的所有飯店,這些飯店會依照城市/國家分類,並顯示n間飯店
        {
            // var currentUser = _userService.GetCurrentUser(); 獲取當前用戶
            //int currentUserId = // 獲取當前用戶ID

            return View();
        }
        
        public IActionResult HotelsFavoritesListToCity(int memberId, string city, string country) 
        { 
            return View(); 
        }

        //public IActionResult HotelsFavoritesListToCity(string CityName)
        //{

        //    return View();
        //}



    }
}
