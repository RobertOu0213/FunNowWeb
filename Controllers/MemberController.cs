﻿using Microsoft.AspNetCore.Mvc;
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

namespace PrjFunNowWeb.Controllers
{
    public class MemberController : Controller
    {
        //串接資料庫
        private readonly FunNowContext _context;

        public MemberController(FunNowContext context)
        {
                _context = context;
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

                // 將memberInfo和token存在Session中
                HttpContext.Session.SetString("MemberInfo", JsonConvert.SerializeObject(loginResponse.memberInfo));
                HttpContext.Session.SetString("Token", loginResponse.token); 

                return Ok(loginResponse);
            }
        }

        //把Session存在View Bag可以給其他Razor頁面使用
        public IActionResult Profile()
        {
            var memberInfoJson = HttpContext.Session.GetString("MemberInfo");
            MemberInfo memberInfo = null;

            if (!string.IsNullOrEmpty(memberInfoJson))
            {
                memberInfo = JsonConvert.DeserializeObject<MemberInfo>(memberInfoJson);
            }

            var token = HttpContext.Session.GetString("Token");

            // 將 MemberInfo 和 Token 傳遞給 View
            ViewBag.MemberInfo = memberInfo;
            ViewBag.Token = token;

            return View(memberInfo);
        }


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
        
        //處理Google登入時驗證成功或失敗時的跳轉頁面
        public async Task<IActionResult> GoogleResponse()
        {
            // 嘗試進行驗證，使用Cookie作為驗證方案
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded) //如果Google驗證失敗，回到MemberController的Login登入頁面，並使用TempData傳遞錯誤訊息
            { 
                TempData["ErrorMessage"] = "Google驗證失敗，請再試一次。";
                return RedirectToAction("Login", "Member");   
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
            return RedirectToAction("Index", "Home");
        }

       //處理登出 
        public IActionResult Logout()
        {
            // 清除 Session 資料
            HttpContext.Session.Remove("GoogleMemberID");
            HttpContext.Session.Remove("GoogleMemberEmail");
            HttpContext.Session.Remove("GoogleMemberFirstName");
            HttpContext.Session.Remove("MemberInfo");
            HttpContext.Session.Remove("Token");

            // 重定向到首頁
            return RedirectToAction("Index", "Home");
        }

       
        public IActionResult Register()
        {
            return View();
        }

        
        public IActionResult MemberInformation()
        {
            return View();
        }

        public IActionResult HostInformation()
        {
            return View();
        }

       
        

    }
}


