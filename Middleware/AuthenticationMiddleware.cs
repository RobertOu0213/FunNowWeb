using PrjFunNowWeb.Models;

namespace PrjFunNowWeb.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var user = context.Session.GetString("MemberID");

            //不用經過middleware，可以允許放行的放在這裡，放controller/Action，最後結尾一定要加上逗點
            var allowedPaths = new List<string> {"/", "/Home/Index", "/Home/Index2", "/Member/Register", "/Member/Login", "/PgHotel/pgHotel", "/ForgetPwd/ForgotPassword", "/Member/LoginWithAPI", "/Member/LoginByGoogle", };
            
           
            var isAllowedPath = allowedPaths.Any(path => context.Request.Path.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(user) && !isAllowedPath)
            {
                context.Session.SetString("AlertMessage", "您必須登入才能訪問該頁面。");
                // 重定向到登入頁面
                context.Response.Redirect("/Member/Login");
                return;
            }

            await _next(context);
        }
    }
}
