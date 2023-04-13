global using Microsoft.AspNetCore.Components.Authorization;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using Blazored.LocalStorage;
global using QuizzAPP.Shared;
global using QuizzAPP.Client.Services;
global using QuizzAPP.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using QuizzAPP.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Logging.AddFilter("Microsoft.AspNetCore.Authorization.*", LogLevel.None);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuizzService, QuizzService>();

await builder.Build().RunAsync();
