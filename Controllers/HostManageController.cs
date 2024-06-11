using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrjFunNowWeb.Models;
using PrjFunNowWeb.Models.DTO;
using PrjFunNowWeb.Models.ViewModel;
using System;

namespace PrjFunNowWeb.Controllers
{
    public class HostManageController : Controller
    {
        private readonly FunNowContext _context;
        private readonly IWebHostEnvironment _env;

        public HostManageController(FunNowContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
                var cityName = _context.Cities.FirstOrDefault(c => c.CityId == hotel.CityId);
                var country = _context.Countries.FirstOrDefault(c => c.CountryId == hotel.City.CountryId);

                return Json(new
                {
                    success = true,
                    message = "Data saved successfully.",
                    updatedHotel = new
                    {

                        hotel.HotelName,
                        CityName = cityName?.CityName,
                        CountryName = country?.CountryName

                    }
                });
            }

            return Json(new { success = false, message = "Invalid model state." });
        }


        public IActionResult HostHotelPhoto(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Home");
            }

            var hotel = (from h in _context.Hotels
                         where h.HotelId == id
                         select new CHotelImageViewModel
                         {
                             HotelId = h.HotelId,
                             HotelName = h.HotelName,
                             CityName = h.City.CityName,
                             CountryName = h.City.Country.CountryName,
                             HotelImage = h.HotelImages.Select(hi => hi.HotelImage1).FirstOrDefault(),
                             AllhotelImages = h.HotelImages.ToList(),
                             AllimageCategoryReferences = h.HotelImages.SelectMany(hi => hi.ImageCategoryReferences).ToList(),
                             AllimageCategories = _context.ImageCategories.ToList()
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
        public async Task<IActionResult> HostHotelPhoto([FromForm] CSaveHotelImageDTO changesData)
        {
            if (changesData == null)
            {
                return BadRequest("No data received.");
            }

            // 保存新照片
            if (changesData.NewImages != null)
            {
                for (int i = 0; i < changesData.NewImages.Count; i++)
                {
                    var imageFile = changesData.NewImages[i];
                    var categoryId = changesData.NewImagesCategories[i];


                    var extension = Path.GetExtension(imageFile.FileName);
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(_env.WebRootPath, "image", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    var hotelImage = new HotelImage
                    {
                        HotelImage1 = fileName,
                        HotelId = changesData.HotelId
                        
                    };

                    hotelImage.ImageCategoryReferences.Add(new ImageCategoryReference
                    {
                        ImageCategoryId = categoryId,
                        HotelImageId = hotelImage.HotelImageId
                    });

                    _context.HotelImages.Add(hotelImage);
                }
            }

            // 更新舊照片類別
            if (changesData.UpdatedImages != null)
            {
                for (int i = 0; i < changesData.UpdatedImages.Count; i++)
                {
                    var imageId = changesData.UpdatedImages[i];
                    var categoryId = changesData.UpdatedImagesCategories[i];

                    var hotelImage = _context.HotelImages.Include(hi => hi.ImageCategoryReferences)
                                                         .FirstOrDefault(hi => hi.HotelImageId == imageId);
                    if (hotelImage != null)
                    {
                        hotelImage.ImageCategoryReferences.Clear();
                        hotelImage.ImageCategoryReferences.Add(new ImageCategoryReference
                        {
                            ImageCategoryId = categoryId
                        });
                    }
                }
            }

            // 删除舊照片
            if (changesData.DeletedImages != null)
            {
                foreach (var imageId in changesData.DeletedImages)
                {
                    var hotelImage = _context.HotelImages.Include(hi => hi.ImageCategoryReferences)
                                                         .FirstOrDefault(hi => hi.HotelImageId == imageId);
                    if (hotelImage != null)
                    {
                        // 删除對應類別
                        var categoryReference = hotelImage.ImageCategoryReferences.FirstOrDefault();
                        if (categoryReference != null)
                        {
                            _context.ImageCategoryReferences.Remove(categoryReference);
                        }

                        // 删除硬碟照片
                        var filePath = Path.Combine(_env.WebRootPath, "image", hotelImage.HotelImage1);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        // 删除DB資料
                        _context.HotelImages.Remove(hotelImage);
                    }
                }
            }

            _context.SaveChanges();

            return Ok(new { success = true, message = "All changes saved successfully" });

        }
      
        public IActionResult HostRoomCreate(int? id)
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
                             HotelImage = h.HotelImages.Select(hi => hi.HotelImage1).FirstOrDefault(),
                             AllRoomTypes = _context.RoomTypes.ToList(),                          
                             AllRoomEquipments = _context.RoomEquipments.ToList(),
                             AllimageCategories = _context.ImageCategories.ToList()
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
        public async Task<IActionResult> HostRoomCreate([FromForm] CRoomCreateDTO roomDto)
        {
            if (roomDto == null)
            {
                return BadRequest("Invalid room data.");
            }

            var room = new Room
            {
                RoomName = roomDto.RoomName,
                RoomTypeId = roomDto.RoomTypeId,
                RoomSize = roomDto.RoomSize,
                MaximumOccupancy = roomDto.RoomPeople,
                RoomPrice = roomDto.RoomPrice,
                Description = roomDto.RoomDescription,
                RoomStatus = true,
                MemberId = 1,
                HotelId = roomDto.HotelId
            };
            _context.Rooms.Add(room);

            foreach (var equipmentId in roomDto.RoomEquipmentIds)
            {
                var roomEquipmentReference = new RoomEquipmentReference
                {
                    RoomId = room.RoomId,
                    RoomEquipmentId = equipmentId
                };

                room.RoomEquipmentReferences.Add(roomEquipmentReference);
            }

            foreach (var imageDto in roomDto.Images)
            {

                var extension = Path.GetExtension(imageDto.ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(_env.WebRootPath, "image", fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageDto.ImageFile.CopyToAsync(stream);
                }

                var image = new RoomImage
                {
                    RoomImage1 = fileName,
                    RoomId = room.RoomId
                };

                room.RoomImages.Add(image);


                image.ImageCategoryReferences.Add(new ImageCategoryReference
                {
                    ImageCategoryId = imageDto.ImageCategoryId,
                    RoomImageId = image.RoomImageId
                });
            }

            _context.SaveChanges();

            return Ok(new { success = true, message = "All changes saved successfully" });

        }

        public IActionResult HostRoomUpdate(int? id)
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
                             HotelImage = h.HotelImages.Select(hi => hi.HotelImage1).FirstOrDefault(),
                             AllRoomTypes = _context.RoomTypes.ToList(),
                             AllRoomEquipments = _context.RoomEquipments.ToList(),
                             AllimageCategories = _context.ImageCategories.ToList(),
                             AllRooms = h.Rooms.Select(room => new Room { RoomId = room.RoomId, RoomName = room.RoomName }).ToList()
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

        [HttpGet]
        public async Task<IActionResult> GetRoomDetails (int roomId)
        {
            var room = _context.Rooms
           .Include(r => r.RoomType)
           .Include(r => r.RoomEquipmentReferences)
           .FirstOrDefault(r => r.RoomId == roomId);

            if (room == null)
            {
                return Json(new { success = false, message = "Room not found" });
            }

            return Ok(new
            {
                success = true,
                data = new
                {
                    room.RoomName,
                    room.RoomTypeId,
                    room.RoomSize,
                    room.MaximumOccupancy,
                    room.RoomPrice,
                    RoomEquipmentIds=room.RoomEquipmentReferences.Select(e => e.RoomEquipmentId),
                    room.Description
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> HostRoomUpdate([FromBody] CRoomSaveDTO roomIn)
        {
            if(roomIn == null)
            {
                return (BadRequest("No data received."));
            }
   
            try
            {
                var room = await _context.Rooms
                    .Include(r => r.RoomEquipmentReferences)
                    .FirstOrDefaultAsync(r => r.RoomId == roomIn.RoomId);

                if (room == null)
                {
                    return Ok(new { success = false, message = "Room not found" });
                }

                room.RoomName = roomIn.RoomName;
                room.RoomTypeId = roomIn.RoomTypeId;
                room.RoomSize = roomIn.RoomSize;
                room.MaximumOccupancy = roomIn.MaximumOccupancy;
                room.RoomPrice = roomIn.RoomPrice;
                room.Description = roomIn.Description;

                _context.RoomEquipmentReferences.RemoveRange(room.RoomEquipmentReferences);

                foreach (var equipmentId in roomIn.RoomEquipmentIds)
                {
                    room.RoomEquipmentReferences.Add(new RoomEquipmentReference { RoomId = room.RoomId, RoomEquipmentId = equipmentId });
                }

                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }

        }


        public IActionResult HostRoomImageUpdate(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Home");
            }

            var hotel = (from h in _context.Hotels
                         where h.HotelId == id
                         select new CHotelImageViewModel
                         {
                             HotelId = h.HotelId,
                             HotelName = h.HotelName,
                             CityName = h.City.CityName,
                             CountryName = h.City.Country.CountryName,
                             HotelImage = h.HotelImages.Select(hi => hi.HotelImage1).FirstOrDefault(),
                             AllhotelImages = h.HotelImages.ToList(),
                             AllimageCategoryReferences = h.HotelImages.SelectMany(hi => hi.ImageCategoryReferences).ToList(),
                             AllimageCategories = _context.ImageCategories.ToList()
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
