using Microsoft.EntityFrameworkCore;
using PrjFunNowWeb.Models;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using PrjFunNowWeb.Models.DTO;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
   .AddCookie()
   .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
   {
       options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientID").Value;
       options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
   });




// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient(); // �K�[HttpClient�A��

builder.Services.AddDbContext<FunNowContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("FunNowConnection")
));

// �K�[ Session �A��
builder.Services.AddSession(options =>
{
    // �]�m Session �� cookie �W��
    options.Cookie.Name = ".YourApp.Session";

    // �]�m Session ���L���ɶ�
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    // �]�m cookie �O���O�u�b HTTPS ������
    options.Cookie.HttpOnly = true;

    // �]�m cookie ���w������
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleReCaptcha"));


var app = builder.Build();

// �t�mHTTP�ШD�޹D
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession(); //���USession �A��
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
