using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models.DTO;

namespace PrjFunNowWeb.Controllers
{
    public class BackSideHotelSystemController : Controller
    {
        public IActionResult Hotel()
        {
            return View();
        }

        [HttpPost]
        public IActionResult backSideHotelSearch([FromBody] BackSideHotelSearchDTO search)
        {
            return Json(search);
        }
 
    }
   
}
