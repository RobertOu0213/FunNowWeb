using Microsoft.AspNetCore.Mvc;
using PrjFunNowWeb.Models.ViewModel;
using PrjFunNowWeb.Models;
using System.Text.Json;

namespace PrjFunNowWeb.Controllers
{
    public class MemberController : Controller
    {
        private readonly FunNowContext _context;

        public MemberController(FunNowContext context)
        {
                _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

         [HttpPost]
         public JsonResult Login(CLoginViewModel vm) //用於處理登入請求並返回 JSON 格式的結果。接受 CLoginViewModel 類型的參數。
        {
                if (ModelState.IsValid) //檢查模型是否有效
                {
                    Member member = _context.Members.FirstOrDefault(m => m.Email == vm.EmailInput && m.Password == vm.PasswordInput);

                    if (member != null) //如果有找到符合資料的會員
                    {
                        string json = JsonSerializer.Serialize(member); //將會員資料序列化為 JSON。
                        HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json); //將 JSON 資料存入 Session。
                        return Json(new { success = true, redirectUrl = Url.Action("Index", "Member") }); //返回成功結果轉到首頁。
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

        public IActionResult Index()
        {
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
                    return View();
                return RedirectToAction("Login");
            }


    }
}


