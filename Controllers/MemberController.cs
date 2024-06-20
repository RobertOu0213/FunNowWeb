using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models.ViewModel;
using PrjFunNowWeb.Models;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using NuGet.Protocol.Plugins;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Newtonsoft.Json;
using NuGet.Protocol;
using PrjFunNowWebApi.Models;
using NuGet.Common;
using System.Net.Http;
using Azure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using DotNetEnv;
using PrjFunNowWeb.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace PrjFunNowWeb.Controllers
{
    public class MemberController : Controller
    {
        //串接資料庫
        private readonly FunNowContext _context;
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _env;

        public MemberController(FunNowContext context, HttpClient httpClient, IWebHostEnvironment env)
        {
            _context = context;
            _httpClient = httpClient;
            _env = env;
        }

        //這個只是呈現登入的頁面
        public IActionResult Login() 
        {
            return View();
        }

        //這個是接收LoginAPI傳回來的資料，存在Session的過程
        [HttpPost]
        public async Task<IActionResult> LoginWithAPI([FromBody] LoginRequestt loginRequest)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7103"); 
                var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("/api/Login/", content);

                if (!response.IsSuccessStatusCode)
                {
                    // 顯示錯誤訊息用的
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorMessage);
                   
                }

                var responseData = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseData);

                if (loginResponse != null)
                {
                    // 將memberID和token存在Session中
                    HttpContext.Session.SetString("MemberID", loginResponse.memberID.ToString());
                    HttpContext.Session.SetString("Token", loginResponse.token);
                    return Ok(loginResponse);
                }
                else
                {
                    return BadRequest("無效的回應格式");
                }
            }
        }


        //把帳號密碼的資訊存在Session
        //[HttpPost]
        //public async Task<IActionResult> GetLoginMember()
        //{
        //    var response = await _httpClient.PostAsJsonAsync("",);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        TempData["Email"] = model.Email;
        //        return RedirectToAction("VerifyOtp");
        //    }

        //    ModelState.AddModelError(string.Empty, "Failed to send OTP");
        //    return View("ForgotPassword");
        //}



        //第三方登入的頁面
        //*參考影片:https://reurl.cc/VzqpKR */
        public async Task LoginByGoogle() //返回類型為Task
        {
            // 向HttpContext發起挑戰，以Google作為驗證提供者，指定Google作為驗證方案
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    //設定挑戰成功後的重定向URI，將使用者導向MemberController的GoogleResponse方法
                    RedirectUri = Url.Action("GoogleResponse","Member")  
                });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            // 嘗試進行驗證，使用Cookie作為驗證方案
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                ViewBag.ErrorMessage = "Google驗證失敗，請再試一次。";
                return View("Login");
            }

            //如果驗證成功
            //從HttpContext.User.Claims中獲取經過Google驗證的使用者資訊,包括電子郵件和姓名。
            var claims = HttpContext.User.Claims;
            var email = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            var name = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;
            var phone = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.MobilePhone)?.Value;
            var birthday = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.DateOfBirth)?.Value;

            // 檢查使用者是否已經存在於資料庫中
            var existingMember = _context.Members.FirstOrDefault(m => m.Email == email);
            if (existingMember == null) // 如果不存在則創建新的使用者
            {
                var newMember = new Member
                {
                    // 放你想從google拿到的使用者資訊
                    Email = email,
                    FirstName = name,
                    Phone = "google登入不用設定手機號碼",
                    Password = "google登入不用設定密碼",
                };
                _context.Members.Add(newMember); //直接幫這些用google登入的使用者註冊進資料庫
                _context.SaveChanges();
                existingMember = newMember; // 設置existingMember為新創建的Member實例
            }

            // 在Session中儲存會員ID
            HttpContext.Session.SetString("GoogleMemberID", existingMember.MemberId.ToString());
            HttpContext.Session.SetString("GoogleMemberEmail", existingMember.Email);
            HttpContext.Session.SetString("GoogleMemberFirstName", existingMember.FirstName);

            //根據角色判斷登入權限
            var roleID = _context.Members.Where(m => m.MemberId == existingMember.MemberId)
                                     .Select(m => m.RoleId)
                                     .FirstOrDefault();

            switch (roleID)
            {
                case 4:
                    ViewBag.ErrorMessage = "您的帳戶已被刪除，請重新註冊";
                    return View("Login");
                case 3:
                    ViewBag.ErrorMessage = "您的帳戶已被鎖定，請洽客服協助解鎖";
                    return View("Login");
                case 2:
                    ViewBag.ErrorMessage = "管理員歡迎你";
                    return RedirectToAction("pgBack_Member", "PgBack_Member");
                case 1:
                    return RedirectToAction("Index", "Home");
                default:
                    ViewBag.ErrorMessage = "角色驗證錯誤";
                    return View("Login");
            }
        }

        //處理登出 
        public IActionResult Logout()
        {
            // 清除 Session 資料
            HttpContext.Session.Remove("GoogleMemberID");
            HttpContext.Session.Remove("GoogleMemberEmail");
            HttpContext.Session.Remove("GoogleMemberFirstName");
            HttpContext.Session.Remove("MemberID");
            HttpContext.Session.Remove("Token");

            // 重定向到首頁
            return RedirectToAction("Index", "Home");
        }

       
        public IActionResult Register()
        {
            return View();
        }

        
        //public IActionResult MemberInformation()
        //{
        //    return View();
        //}



        public IActionResult HostInformation()
        {
           
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
                var extension = Path.GetExtension(imageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(_env.WebRootPath, "image", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                member.Image = $"/image/{fileName}";
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


