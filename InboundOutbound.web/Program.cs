using InboundOutbound.web.Service;
using InboundOutbound.web.Service.IService;
using InboundOutbound.web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddHttpClient<IOutboundService, OutboundService>();
builder.Services.AddHttpClient<IInboundService, InboundService>();



Sd.UserAPIBase = builder.Configuration["ServiceUrls:UserAPI"];
Sd.OutboundAPIBase = builder.Configuration["ServiceUrls:OutboundAPI"];
Sd.InboundAPIBase = builder.Configuration["ServiceUrls:InboundAPI"];
Sd.EmailTrailAPIBase = builder.Configuration["ServiceUrls:EmailTrailAPI"];


builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IOutboundService, OutboundService>();
builder.Services.AddScoped<IInboundService, InboundService>();



//var emailConfig = builder.Configuration
//        .GetSection("Smtp")
//        .Get<EmailConfig>();
//builder.Services.AddSingleton(emailConfig);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        //options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/User/Login";
        options.AccessDeniedPath = "/User/AccessDenied";
    });

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");
pattern: "{controller=User}/{action=Login}/{id?}");
app.Run();
