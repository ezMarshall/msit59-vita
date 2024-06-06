using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using msit59_vita.Models;
using SignalRChat.Hubs;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("VitaContext");
builder.Services.AddDbContext<VitaContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<VitaUser>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._%+-@";
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<VitaContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.Cookie.Name = "GoogleLoginCookie";
        options.LoginPath = "/Account/GoogleLogin";
    })
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
        options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
		options.Scope.Add("profile");
		options.Scope.Add("email");
		options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Name, "name");
		options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Email, "email");
	});

builder.Services.AddScoped<UserManager<VitaUser>, UserManager<VitaUser>>();
builder.Services.AddScoped<SignInManager<VitaUser>, SignInManager<VitaUser>>();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ManagerHome}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chatHub");
app.Run();
