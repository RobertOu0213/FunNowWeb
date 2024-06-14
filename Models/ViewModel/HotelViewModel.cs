namespace PrjFunNowWeb.Models.ViewModel
{
    public class HotelViewModel
    {
        public IEnumerable<HotelType> HotelTypes { get; set; }
        public IEnumerable<HotelEquipment> HotelEquipments { get; set; }
        public IEnumerable<City> Cities { get; set; }
        public IEnumerable<RoomEquipment> RoomEquipments { get; set; }
    }
}
