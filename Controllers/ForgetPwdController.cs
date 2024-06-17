using Microsoft.AspNetCore.Mvc;

namespace PrjFunNowWeb.Controllers
{
    public class ForgetPwdController : Controller
    {
        private readonly HttpClient _httpClient;

        public ForgetPwdController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

    }
}
