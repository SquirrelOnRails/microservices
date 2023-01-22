using Mango.Web;
using Mango.Web.Services;
using Mango.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

#region dependencies
IdentityModelEventSource.ShowPII = true;

// Add services to the container.
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IShoppingCartService, ShoppingCartService>();
builder.Services.AddHttpClient<ICouponService, CouponService>();

SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
SD.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];
SD.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClientId = "mango";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.SaveTokens = true;

        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.ClaimActions.MapJsonKey("sub", "sub", "sub");

        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "role";
        options.Scope.Add("mango");
    });
#endregion

var app = builder.Build();

#region middleware
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
#endregion

app.Run();
