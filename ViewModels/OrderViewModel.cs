namespace PrjFunNowWeb.ViewModels
{
    public class OrderViewModel
    {
        public string GuestName { get; set; }
        public int OrderNumber { get; set; }
        public string HotelName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }

        }
}
