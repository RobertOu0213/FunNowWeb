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
        public async Task LoginByGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    //重新導向MemberController的GoogleResponse Action方法
                    RedirectUri = Url.Action("GoogleResponse","Member")  
                });
        }
        
        //處理Google登入時驗證成功或失敗時的跳轉頁面
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result.Succeeded) //如果驗證成功，就跳轉到HomeController的Index首頁
            {
                return RedirectToAction("Index", "Home");
            }

            // 如果Google驗證失敗，回到MemberController的Login登入，並使用TempData傳遞錯誤訊息
            TempData["ErrorMessage"] = "Google驗證失敗，請再試一次。";
            return RedirectToAction("Login", "Member"); 
        }



        [HttpPost]
        public JsonResult Login(CLoginViewModel vm) //用於處理登入請求並返回 JSON 格式的結果。接受 CLoginViewModel 類型的參數vm。
        {
            if (ModelState.IsValid) //檢查模型是否有效
            {
                Member member = _context.Members.FirstOrDefault(m => m.Email == vm.EmailInput && m.Password == vm.PasswordInput);

                if (member != null) //如果有找到符合資料的會員
                {
                    string json = JsonSerializer.Serialize(member); //將會員資料序列化為 JSON
                    HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json); //將 JSON 資料存入 Session
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") }); //返回成功結果轉到首頁
                }
            }

            return Json(new { success = false }); //登入失敗，返回失敗結果。
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


