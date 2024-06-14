namespace PrjFunNowWeb.Models.DTO
{
    public class CRoomCreateDTO
    {
      
        public string RoomName { get; set; }
        public int RoomTypeId { get; set; }
        public decimal RoomSize { get; set; }
        public int RoomPeople { get; set; }
        public decimal RoomPrice { get; set; }
        
        public List<int> RoomEquipmentIds { get; set; }
        public string RoomDescription { get; set; }
        public List<CRoomCreateImageDTO> Images { get; set; }

        public int HotelId { get; set; }
    }
}
