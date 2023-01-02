using Mango.Web;
using Mango.Web.Services;
using Mango.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region dependencies
// Add services to the container.
builder.Services.AddHttpClient<IProductService, ProductService>();
SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllersWithViews();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
#endregion

app.Run();
