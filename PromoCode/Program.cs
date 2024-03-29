using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using PromoCode.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DBPromoCode>(options =>
    options.UseSqlServer("Server=ms-sql-6.in-solve.ru;Database=1gb_x5basequest;uid=1gb_rcpsec;pwd=48ac9e62ps;Encrypt=False;TrustServerCertificate=False;"));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => //CookieAuthenticationOptions
        {
            options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Home/Login");
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
            options.SlidingExpiration = true;

        });
builder.Services.AddMemoryCache();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyApp.Session";
    options.IdleTimeout = TimeSpan.FromDays(7);
    options.Cookie.IsEssential = true;
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

app.UseAuthentication();��� // ��������������
app.UseAuthorization();���� // �����������


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");
app.UseSession();
app.Run();
