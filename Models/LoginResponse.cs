
using PrjFunNowWebApi.Models;

namespace PrjFunNowWeb.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public MemberInfo memberInfo { get; set; }
    }
}
