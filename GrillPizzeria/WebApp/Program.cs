using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using ClassLibrary.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApp.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<GrillPizzeriaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnStr"));
});

// Add services to the container
builder.Services.AddControllersWithViews();

// Add session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Korisnik/SignIn";
        options.LogoutPath = "/Korisnik/SignOut";
        options.AccessDeniedPath = "/Korisnik/Forbidden";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register repositories
builder.Services.AddScoped<HranaRepository>();
builder.Services.AddScoped<KorisnikRepository>();
builder.Services.AddScoped<LogRepository>();
builder.Services.AddScoped<NarudzbaRepository>();
builder.Services.AddScoped<IKategorijaHraneRepository, KategorijaHraneRepository>();
builder.Services.AddScoped<IAlergenRepository, AlergenRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
