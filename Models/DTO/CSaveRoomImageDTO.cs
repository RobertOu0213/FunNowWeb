namespace PrjFunNowWeb.Models.DTO
{
    public class CSaveRoomImageDTO
    {
        public int RoomId { get; set; }
        public List<IFormFile> NewImages { get; set; }
        public List<int> NewImagesCategories { get; set; }
        public List<int> UpdatedImages { get; set; }
        public List<int> UpdatedImagesCategories { get; set; }
        public List<int> DeletedImages { get; set; }
    }
}
