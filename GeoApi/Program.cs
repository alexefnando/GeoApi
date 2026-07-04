var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("geoserver", client =>
{
    client.Timeout = TimeSpan.FromSeconds(120);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();