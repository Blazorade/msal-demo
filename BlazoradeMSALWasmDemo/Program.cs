using Blazorade.Msal.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazoradeMSALWasmDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBlazoradeMsal((provider, options) =>
            {
                var config = provider.GetRequiredService<IConfiguration>();

                var myAppConfig = config.GetSection("myApp");
                options.ClientId = myAppConfig.GetValue<string>("clientId");
                options.Authority = myAppConfig.GetValue<string>("authority");
                options.InteractiveLoginMode = InteractiveLoginMode.Popup;
                options.RedirectUrl = "/msal-demo/login";
                options.PostLogoutUrl = "/msal-demo/loggedout";
            });

            await builder.Build().RunAsync();
        }
    }
}
