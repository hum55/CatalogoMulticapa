using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Infrastructure.Repositories;
using CatalogoApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

var dataPath = Path.Combine(builder.Environment.ContentRootPath, "data");

builder.Services.AddSingleton<IItemRepository>(
    new JsonItemRepository(Path.Combine(dataPath, "items.json")));
builder.Services.AddSingleton<IUserRepository>(
    new JsonUserRepository(Path.Combine(dataPath, "users.json")));
builder.Services.AddSingleton<IReviewRepository>(
    new JsonReviewRepository(Path.Combine(dataPath, "reviews.json")));

builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ReviewService>();

if (!builder.Environment.IsDevelopment())
{
    var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://127.0.0.1:5006";
    builder.WebHost.UseUrls(urls);
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
