using LiveEditor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<FtpConversions>();
builder.Services.AddSingleton<KellerAg.Shared.IoT.Converters.IConvert, KellerAg.Shared.IoT.Converters.IoTConvert>();
await builder.Build().RunAsync();
