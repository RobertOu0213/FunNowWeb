using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Angular_test()
        {
            return View();
        }
    }
}
