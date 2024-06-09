using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.Models.ViewModel;

namespace PrjFunNowWeb.Controllers
{
    public class HostManageController : Controller
    {
        private readonly FunNowContext _context;

        public HostManageController(FunNowContext context)
        {
            _context = context;
        }
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult HostHotelInfo(int? id)
        {
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
                             HotelImage = h.HotelImages.Select(hi => hi.HotelImage1).FirstOrDefault(),
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

        [HttpPost]
        public IActionResult HostHotelInfo(CsingleHotelViewModel hotelIn)
        {
            if (hotelIn != null)
            {
                var hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == hotelIn.HotelId);
                hotel.HotelName = hotelIn.HotelName;
                hotel.HotelAddress = hotelIn.HotelAddress;
                hotel.HotelPhone = hotelIn.HotelPhone;
                hotel.HotelDescription = hotelIn.HotelDescription;
                hotel.LevelStar = hotelIn.LevelStar;

                var hotelType = _context.HotelTypes.FirstOrDefault(ht => ht.HotelTypeId == hotelIn.HotelType.HotelTypeId);
                hotel.HotelTypeId = hotelType.HotelTypeId;

                // 處理城市        
                var city = _context.Cities.FirstOrDefault(c => c.CityId == Convert.ToInt64(hotelIn.CityName));
                if (city != null)
                {
 
                    hotel.CityId = city.CityId;
                }

                _context.HotelEquipmentReferences.RemoveRange(_context.HotelEquipmentReferences.Where(her => her.HotelId == hotelIn.HotelId));
                foreach (var equipmentId in hotelIn.HotelEquipmentsNumber)
                {
                    var hotelEquipmentReference = new HotelEquipmentReference
                    {
                        HotelId = hotelIn.HotelId,
                        HotelEquipmentId = equipmentId
                    };

                    _context.HotelEquipmentReferences.Add(hotelEquipmentReference);

                }

                _context.SaveChanges();
                return RedirectToAction("HostHotelInfo", new { id = hotelIn.HotelId });
            }

            hotelIn.AllHotelTypes = _context.HotelTypes.ToList();
            hotelIn.AllHotelEquipments = _context.HotelEquipments.ToList();
            return View(hotelIn);
        }

    }
}
