using Microsoft.EntityFrameworkCore;
using PrjFunNowWeb.Models;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

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




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession(); //���USession �A��
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=test}/{id?}");

app.Run();
