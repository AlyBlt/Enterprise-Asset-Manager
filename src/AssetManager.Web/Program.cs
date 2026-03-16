using Microsoft.AspNetCore.HttpOverrides;
using AssetManager.Web.Handlers;
using AssetManager.Web.Interfaces;
using AssetManager.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<AuthHeaderHandler>();

// 1. Authentication Yapýlandýrmasý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "AssetManager.Auth";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

// 2. API URL Ayarýný Al
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://api:8080/";



// 3. HttpClient Kayýtlarý
builder.Services.AddHttpClient<IHomeApiService, HomeApiService>(client => { client.BaseAddress = new Uri(apiBaseUrl); }).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddHttpClient<IDashboardApiService, DashboardApiService>(client => { client.BaseAddress = new Uri(apiBaseUrl); }).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddHttpClient<IAuthApiService, AuthApiService>(client => { client.BaseAddress = new Uri(apiBaseUrl); });
builder.Services.AddHttpClient<IAssetApiService, AssetApiService>(client => { client.BaseAddress = new Uri(apiBaseUrl); }).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddHttpClient<IDepartmentApiService, DepartmentApiService>(client => { client.BaseAddress = new Uri(apiBaseUrl); }).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddHttpClient<IAdminApiService, AdminApiService>(client => { client.BaseAddress = new Uri(apiBaseUrl); }).AddHttpMessageHandler<AuthHeaderHandler>();

// 4. Diðer Servisler
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();