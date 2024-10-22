using Infrastructure.Configuration;
using Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:7064");

builder.SeriLogConfiguration();


//Configure serilog


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ApiServiceConfiguration(); ;

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseMiddleware<CookieRenewalMiddleware>();

app.Run();
