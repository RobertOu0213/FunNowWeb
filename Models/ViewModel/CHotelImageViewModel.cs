namespace PrjFunNowWeb.Models.ViewModel
{
    public class CHotelImageViewModel
    {

        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }

        public string HotelImage { get; set; }
        public List<HotelImage> AllhotelImages { get; set; }

        public List<ImageCategoryReference> AllimageCategoryReferences { get; set; }
      
        public List<ImageCategory> AllimageCategories { get; set; }
    }
}
