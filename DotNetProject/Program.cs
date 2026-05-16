using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Web.DataAccess.Data;
using Web.DataAccess.Repository;
using Web.DataAccess.Repository.IRepository;
using Web.Models;
using Web.Utilities;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));

// Services

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // 🔑 REQUIRED FOR IDENTITY

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"];

app.UseRouting();

app.UseAuthentication(); // 🔑
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customers}/{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages(); // 🔑 REQUIRED FOR /Identity/*

app.Run();
