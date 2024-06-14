namespace PrjFunNowWeb.Models.DTO
{
    public class CRoomSaveDTO
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomSize { get; set; }
        public int MaximumOccupancy { get; set; }
        public decimal RoomPrice { get; set; }
        public List<int> RoomEquipmentIds { get; set; }
        public string Description { get; set; }
    }
}
