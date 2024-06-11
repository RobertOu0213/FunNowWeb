using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models.ViewModel;
using PrjFunNowWeb.Models;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            HttpContext.Session.SetString("MemberID", existingMember.MemberId.ToString());
            HttpContext.Session.SetString("MemberFirstName", existingMember.FirstName);
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Logout()
        {
            // 清除 Session 資料
            HttpContext.Session.Remove("MemberID");
            HttpContext.Session.Remove("MemberFirstName");
            
            // 重定向到首頁
            return RedirectToAction("Index", "Home");
        }

        //一般帳號密碼+JWT驗證，改成用API的方式串接
        //[HttpPost]
        //public JsonResult Login(CLoginViewModel vm) //用於處理登入請求並返回 JSON 格式的結果。接受 CLoginViewModel 類型的參數vm。
        //{
        //    if (ModelState.IsValid) //檢查模型是否有效
        //    {
        //        Member member = _context.Members.FirstOrDefault(m => m.Email == vm.EmailInput && m.Password == vm.PasswordInput);

        //        if (member != null) //如果有找到符合資料的會員
        //        {
        //            string json = JsonSerializer.Serialize(member); //將會員資料序列化為 JSON
        //            HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json); //將 JSON 資料存入 Session
        //            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") }); //返回成功結果轉到首頁
        //        }
        //    }

        //    return Json(new { success = false }); //登入失敗，返回失敗結果。
        //}


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


