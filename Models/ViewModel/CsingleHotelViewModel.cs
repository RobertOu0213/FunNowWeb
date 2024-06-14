using Microsoft.Identity.Client;

namespace PrjFunNowWeb.Models.ViewModel
{
    public class CsingleHotelViewModel
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string HotelImage { get; set; }
        public string HotelAddress { get; set; }
        public string HotelPhone { get; set; }
        public string HotelDescription { get; set; }

        public int? LevelStar { get; set; }
        public List<HotelEquipment> AllHotelEquipments { get; set; }
        public List<HotelEquipmentReference> HotelEquipments { get; set; }
        public List<HotelType> AllHotelTypes { get; set; }
        public HotelType HotelType { get; set; }
        public List<int> HotelEquipmentsNumber { get; set; }

        public List<RoomType> AllRoomTypes { get; set; }
        public List<RoomEquipment> AllRoomEquipments { get; set; }

        public List<ImageCategory> AllimageCategories { get; set; }

        public List<Room> AllRooms { get; set; }
    }
}

