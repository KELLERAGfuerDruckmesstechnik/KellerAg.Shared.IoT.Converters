using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoBlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton<FtpConversions>();
            builder.Services.AddSingleton<KellerAg.Shared.IoT.Converters.IConvert, KellerAg.Shared.IoT.Converters.IoTConvert>();

            await builder.Build().RunAsync();
        }
    }
}
