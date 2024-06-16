using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PrjFunNowWeb.Models.louie_dto;
using PrjFunNowWeb.Models.louie_viewmodel;
using PrjFunNowWeb.Models.louie_helper;
using System.Threading.Tasks;
using System.Web;

namespace PrjFunNowWeb.Controllers
{
    public class PgHotelController : Controller
    {
        private readonly HttpClient _httpClient;

        public PgHotelController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> pgHotel(int hotelId = 17, string checkInDate = "2024-06-29", string checkOutDate = "2024-06-30")
        {
            //var secretKey = "my_secret_key123"; // 确保长度为16字节
            //hotelId = HttpUtility.UrlDecode(hotelId);
            //checkInDate = HttpUtility.UrlDecode(checkInDate);
            //checkOutDate = HttpUtility.UrlDecode(checkOutDate);

            //var decryptedHotelId = int.Parse(EncryptionHelper.Decrypt(hotelId, secretKey));
            //var decryptedCheckInDate = EncryptionHelper.Decrypt(checkInDate, secretKey);
            //var decryptedCheckOutDate = EncryptionHelper.Decrypt(checkOutDate, secretKey);

            var url = $"https://localhost:7103/api/pgHotel/{hotelId}?checkInDate={checkInDate}&checkOutDate={checkOutDate}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var hotelDetailJson = await response.Content.ReadAsStringAsync();
            var hotelDetail = JsonConvert.DeserializeObject<pgHotel_HotelDetailDTO>(hotelDetailJson);

            if (hotelDetail == null)
            {
                return NotFound();
            }

            if (hotelDetail.Rooms == null)
            {
                hotelDetail.Rooms = new List<pgHotel_RoomDTO>();
            }

            if (hotelDetail.SimilarHotels == null)
            {
                hotelDetail.SimilarHotels = new List<pgHotel_SimilarHotelsDTO>();
            }

            hotelDetail.CheckInDate = checkInDate;
            hotelDetail.CheckOutDate = checkOutDate;

            var viewModel = new pgHotel_HotelDetailViewModel
            {
                HotelDetail = hotelDetail
            };

            return View(viewModel);
        }


    }
}
