using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BattleShip.App;
using BattleShip.App.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//builder.Services.AddSingleton<GameState>();
builder.Services.AddScoped<GameState>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    return new GameState(httpClient);
});


await builder.Build().RunAsync();
