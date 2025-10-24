using BookMediaDiscoverer;
using BookMediaDiscoverer.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register Both Book and Movie Services
builder.Services.AddScoped<IBookService, GoogleBooksService>();
builder.Services.AddScoped<IMovieService, OMDbService>();

await builder.Build().RunAsync();