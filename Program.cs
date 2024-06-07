using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PrjFunNowWeb.Models;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

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


// Add SignalR client services if needed for SignalR client side (optional)
builder.Services.AddSignalR();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseSession(); //���USession �A��
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HostManage}/{action=Home}/{id?}");

// �t�m���ѥH��� Angular ����
app.MapFallbackToFile("/dist/fun-now-angular1/index.html");
app.Run();
