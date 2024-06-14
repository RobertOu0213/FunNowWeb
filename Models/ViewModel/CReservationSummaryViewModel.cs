namespace PrjFunNowWeb.Models.ViewModel
{
    public class CReservationSummaryViewModel
    {
        public int OrderDetailID { get; set; }
        public string HotelName { get; set; }
        public string RoomType { get; set; }
        public string RoomName { get; set; }
        public decimal RoomPrice { get; set; }
        public string CityName { get; set; }
        public int AllCommentsCount { get; set; }
        public int LevelStar { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int RoomID { get; set; }
        public int MaximumOccupancy { get; set; }
        public string RoomImage { get; set; }
        public int AllOrderDetailsCount { get; set; }
        public int NumberOfNights => (CheckOutDate - CheckInDate).Days;

        public int MemberID { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }    
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
