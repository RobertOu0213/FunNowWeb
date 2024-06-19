namespace PrjFunNowWeb.Models.DTO
{
    public class BackSideHotelSearchDTO
    {
        public int? HotelID { get; set; } = 0;
        public string? keyword { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string HotelAddress { get; set; } = string.Empty;
        public string HotelPhone { get; set; } = string.Empty;
        public int? HotelIsActive { get; set; } = 0;
        public string? sortBy { get; set; }
        public string? sortType { get; set; } = "asc";
 
    }
}
