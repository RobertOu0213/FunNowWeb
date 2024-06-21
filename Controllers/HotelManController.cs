using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.Models.ViewModel;
using PrjFunNowWeb.Models.DTO;


namespace PrjFunNowWeb.Controllers
{
    public class HotelManController : Controller
    {
        private readonly FunNowContext _context;
        public HotelManController(FunNowContext context)
        {
            _context = context;
        }

        public IActionResult HotelMessenage()
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
            else
            {
                var existingMember = _context.Members
                    .Where(x => x.MemberId == Convert.ToInt32(userID))
                    .FirstOrDefault();

                if (existingMember == null)
                {
                    return NotFound("Member not found");
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


            return View();

        }
        public IActionResult Hoteldaily(int? id)
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

            if (id == null)
            {
                // Redirect to Home method or homepage
                return RedirectToAction("Home", "YourControllerName"); // Replace YourControllerName with the actual controller name
            }

            var hotel = (from h in _context.Hotels
                         where h.HotelId == id
                         select new CsingleHotelViewModel
                         {
                             HotelId = h.HotelId,
                             HotelName = h.HotelName,
                             CityName = h.City.CityName,
                             CountryName = h.City.Country.CountryName,
                             HotelAddress = h.HotelAddress,
                             HotelPhone = h.HotelPhone,
                             HotelDescription = h.HotelDescription,
                             LevelStar = h.LevelStar,
                             HotelImage = h.HotelImages.FirstOrDefault() != null ? h.HotelImages.FirstOrDefault().HotelImage1 : null,
                             AllHotelTypes = _context.HotelTypes.ToList(),
                             HotelType = h.HotelType,
                             AllHotelEquipments = _context.HotelEquipments.ToList(),
                             HotelEquipments = h.HotelEquipmentReferences.ToList()
                         }).FirstOrDefault();

            if (hotel == null)
            {
                return NotFound();
            }

            hotel.HotelImage = hotel.HotelImage != null && (hotel.HotelImage.StartsWith("http://") || hotel.HotelImage.StartsWith("https://"))
                             ? hotel.HotelImage
                             : $"{hotel.HotelImage}";

            ViewBag.HideHotelEditItems = true; // 根據需要設置顯示或隱藏

            return View(hotel);
        }
        public IActionResult Daily(int? id)
        {
            //env test
            //Env.Load();
            //string databaseUrl = Environment.GetEnvironmentVariable("API_KEY");

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

            if (id == null)
            {
                return RedirectToAction("Home");
            }


            var hotel = (from h in _context.Hotels
                         where h.HotelId == id
                         select new CsingleHotelViewModel
                         {
                             HotelId = h.HotelId,
                             HotelName = h.HotelName,
                             CityName = h.City.CityName,
                             CountryName = h.City.Country.CountryName,
                             HotelAddress = h.HotelAddress,
                             HotelPhone = h.HotelPhone,
                             HotelDescription = h.HotelDescription,
                             LevelStar = h.LevelStar,
                             HotelImage = h.HotelImages.Select(hi => hi.HotelImage1).FirstOrDefault() ?? "default_image_url",
                             AllHotelTypes = _context.HotelTypes.ToList(),
                             HotelType = h.HotelType,
                             AllHotelEquipments = _context.HotelEquipments.ToList(),
                             HotelEquipments = h.HotelEquipmentReferences.ToList()
                         }).FirstOrDefault();

            if (hotel == null)
            {
                return NotFound();
            }

            hotel.HotelImage = hotel.HotelImage != null && (hotel.HotelImage.StartsWith("http://") || hotel.HotelImage.StartsWith("https://"))
                             ? hotel.HotelImage
                             : $"{hotel.HotelImage}";

            return View(hotel);
        }

    }
}
