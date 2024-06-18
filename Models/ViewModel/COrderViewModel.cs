namespace PrjFunNowWeb.Models.ViewModel
{
    public class COrderViewModel
    {
        public List<int> OrderDetailsID { get; set; }
        public int MemberID { get; set; }
        public int OrderStatusID { get; set; }
        public int PaymentStatusID { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string GuestLastName { get; set; }
        public string GuestFirstName { get; set; }
        public string GuestEmail { get; set; }
    }
}
