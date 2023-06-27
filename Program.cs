using BitocCore1;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<BitcoinService>();
// добавление кэширования
builder.Services.AddMemoryCache();

var app = builder.Build();

app.MapGet("/count", (BitcoinService bitcoinService) =>
{
    uint count = (bitcoinService as IPairsProvider).Count();
    if (count != 0) return count.ToString();
    return " count = 0 ";
});

app.MapGet("/GetPairs/{page}", async (int page, BitcoinService bitcoinService) =>
{
    var list = (bitcoinService as IPairsProvider).GetPairs(page);
    if (list != null) return JsonConvert.SerializeObject(list); //  ;
    return " list = 0 ";
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, 
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
