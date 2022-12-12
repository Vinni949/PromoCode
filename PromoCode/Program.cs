using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using PromoCode.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DBPromoCode>(options =>
    options.UseSqlServer("Server=ms-sql-6.in-solve.ru;Database=1gb_base5quest;uid=1gb_rcpsec;pwd=48ac9e62ps;Encrypt=False;TrustServerCertificate=False;"));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => //CookieAuthenticationOptions
        {
            options.ExpireTimeSpan = TimeSpan.FromHours(9);
            options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Home/Login");
        });
var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin());
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();    // аутентификация
app.UseAuthorization();     // авторизация


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
