using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models;

namespace PrjFunNowWeb.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult platform_comment()
        {
            return View();
        }

        public IActionResult comment_page()
        {
            return View();
        }


        public IActionResult Angular_comment_page()
        {
            return View();
        }

        public IActionResult Angular_membercomment()
        {
            return View();
        }

        public IActionResult Angular_plaform()
        {
            return View();
        }

        public IActionResult GetComments(int hotelId)
        {
            // 從資料庫獲取數據
           var comments = new List<Comment>();
           
            return Json(comments);
        }
    }
}
