using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrjFunNowWeb.Models;

namespace PrjFunNowWeb.Controllers
{
    public class HostInformationController : Controller
    {

        //串接資料庫
        private readonly FunNowContext _context;
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _env;

        public HostInformationController(FunNowContext context, HttpClient httpClient, IWebHostEnvironment env)
        {
            _context = context;
            _httpClient = httpClient;
            _env = env;
        }

        public IActionResult HostInformationn()
        {
            var userID = HttpContext.Session.GetString("MemberID");
            if (string.IsNullOrEmpty(userID))
            {

                userID = HttpContext.Session.GetString("GoogleMemberID");
                if (string.IsNullOrEmpty(userID))
                {
                    return RedirectToAction("Login", "Member");
                }
            }
            else
            {
                var existingMember = _context.Members
                    .Where(x => x.MemberId == Convert.ToInt32(userID))
                    .FirstOrDefault();

                if (existingMember == null)
                {
                    return NotFound("Member not found");
                }
            }

            var member = _context.Members
                .Where(x => x.MemberId == Convert.ToInt32(userID))
                .Select(x => new { x.MemberId, x.FirstName, x.LastName })
                .FirstOrDefault();

            if (member == null)
            {
                return NotFound("Member not found");
            }

            ViewBag.MemberID = member.MemberId;
            ViewBag.FirstName = member.FirstName;
            ViewBag.LastName = member.LastName;

            return View();
        }


        //修改房東所有個人資料
        [HttpPut("HostMemberEdit/{id}")]
        public async Task<IActionResult> HostMemberEdit(int id, [FromForm] HostMemberEditDTO edit, IFormFile imageFile)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return BadRequest("一開始資料庫就沒有這個會員");
            }

            member.FirstName = edit.FirstName;
            member.LastName = edit.LastName;
            member.Phone = edit.Phone;
            member.Birthday = edit.Birthday;
            member.CityId = edit.CityId;
            member.MemberAddress = edit.MemberAddress;
            member.Introduction = edit.Introduction;

            if (imageFile != null && imageFile.Length > 0)
            {
                // 保存舊的圖片路徑
                var oldImagePath = member.Image;

                // 處理新圖片
                var extension = Path.GetExtension(imageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(_env.WebRootPath, "image", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // 更新新的圖片路徑
                member.Image = $"/image/{fileName}";

                // 刪除舊圖片
                if (!string.IsNullOrEmpty(oldImagePath))
                {
                    var oldFilePath = Path.Combine(_env.WebRootPath, oldImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return BadRequest("你在把更新資料存進資料庫時找不到這個會員了");
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { Message = "會員資料修改成功", ImageUrl = member.Image });
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }

        public class HostMemberEditDTO
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }
            public DateTime? Birthday { get; set; }
            public int? CityId { get; set; }
            public string MemberAddress { get; set; }
            public string Introduction { get; set; }
        }
    }
}
