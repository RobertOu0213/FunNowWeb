﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PrjFunNowWeb.Models;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using PrjFunNowWeb.Models.DTO;
using PrjFunNowWeb.Middleware;

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

builder.Services.AddHttpClient(); // 添加HttpClient服務

builder.Services.AddDbContext<FunNowContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("FunNowConnection")
));

// 添加 HttpClient 服务
builder.Services.AddHttpClient();
// 添加 Session 服務
builder.Services.AddSession(options =>
{
    // 設置 Session 的 cookie 名稱
    options.Cookie.Name = ".YourApp.Session";

    // 設置 Session 的過期時間
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    // 設置 cookie 是不是只在 HTTPS 中有效
    options.Cookie.HttpOnly = true;

    // 設置 cookie 的安全等級
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.Cookie.IsEssential = true;
});

builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleReCaptcha"));

builder.Services.AddCors(options =>
{
    // 定義允許特定來源的策略
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:7103") // 替換為你的前端域名
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});
// Add SignalR client services if needed for SignalR client side (optional)
builder.Services.AddSignalR();

//Angular用
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddControllersWithViews();




builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// 配置 CORS 策略
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// 配置HTTP請求管道
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

app.UseSession(); //註冊Session 服務
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

//app.UseMiddleware<AuthenticationMiddleware>();

// 使用 CORS
app.UseCors("AllowAllOrigins");
app.UseAuthorization();


app.UseCors("AllowAngularApp");


app.MapControllerRoute(
    name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");
//pattern: "{controller=HotelCreate}/{action=HotelInfo}/{id?}");

//// 配置路由以支持 Angular 路由
//app.MapFallbackToFile("/dist/fun-now-angular1/index.html");
app.Run();
