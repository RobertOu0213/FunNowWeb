using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PrjFunNowWeb
{
    public class AuthTokenFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 檢查原始請求是否已包含 token
            var tokenInQueryString = context.HttpContext.Request.Query.ContainsKey("token");

            // 如果請求不包含 token,則進行重新導向
            if (!tokenInQueryString)
            {
                var token = context.HttpContext.Session.GetString("Token");

                // 調試代碼
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token is null or empty");
                }
                else
                {
                    Console.WriteLine($"Token value: {token}");
                }

                if (!string.IsNullOrEmpty(token))
                {
                    var queryString = context.HttpContext.Request.QueryString.HasValue
                        ? context.HttpContext.Request.QueryString.Value
                        : string.Empty;

                    var url = $"{context.HttpContext.Request.Path.Value}?token={token}{queryString}";
                    context.Result = new RedirectResult(url);
                }
            }
            else
            {
                // 如果請求已包含 token,則繼續執行 Action 方法
                base.OnActionExecuting(context);
            }
        }
    }
}
