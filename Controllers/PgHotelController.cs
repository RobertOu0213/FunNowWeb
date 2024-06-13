using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PrjFunNowWeb.Models.louie_dto;
using PrjFunNowWeb.Models.louie_viewmodel;

namespace PrjFunNowWeb.Controllers
{
    public class PgHotelController : Controller
    {
        private readonly HttpClient _httpClient;

        public PgHotelController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> pgHotel(int hotelId=7, string checkInDate = null, string checkOutDate = null)//預設
        {
            // 调用API获取数据
            var url = $"https://localhost:7103/api/pgHotel/{hotelId}?checkInDate={checkInDate}&checkOutDate={checkOutDate}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                // 处理错误情况
                return NotFound();
            }

            var hotelDetailJson = await response.Content.ReadAsStringAsync();
            var hotelDetail = JsonConvert.DeserializeObject<pgHotel_HotelDetailDTO>(hotelDetailJson);

            // 检查 hotelDetail 是否为空
            if (hotelDetail == null)
            {
                return NotFound();
            }

            // 检查 Rooms 是否为空
            if (hotelDetail.Rooms == null)
            {
                hotelDetail.Rooms = new List<pgHotel_RoomDTO>();
            }

            // 检查 SimilarHotels 是否为空
            if (hotelDetail.SimilarHotels == null)
            {
                hotelDetail.SimilarHotels = new List<pgHotel_SimilarHotelsDTO>();
            }

            hotelDetail.CheckInDate = checkInDate;
            hotelDetail.CheckOutDate = checkOutDate;

            // 将数据传递给视图
            var viewModel = new pgHotel_HotelDetailViewModel
            {
                HotelDetail = hotelDetail
            };

            return View(viewModel);
        }
    }
}
