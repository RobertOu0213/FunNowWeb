namespace PrjFunNowWeb.Models.DTO
{
    public class COrderSuccessEmailDTO
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string RoomName { get; set; }
        public decimal RoomPrice { get; set; }
        public string RoomType { get; set; }
        public int? GuestNumber { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string HotelPhone { get; set; }
        public int? LevelStar { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string HotelImage { get; set; }

        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public decimal TotalPrice { get; set; }
        public int NumberOfDays { get; set; }
    }
}
