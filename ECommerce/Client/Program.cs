using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ECommerce.Client.Extension;
using System.Globalization;
using BlazorStrap;

namespace ECommerce.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBootstrapCss();

            builder.Services.AddHttpClient("ECommerce.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ECommerce.ServerAPI"));

            builder.Services.AddApiAuthorization();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            //await builder.Build().RunAsync();

            var host = builder.Build();

            var jsInterlop = host.Services.GetRequiredService<IJSRuntime>();

            var result = await jsInterlop.InvokeAsync<string>("blazorCulture.get");

            if (result != null)
            {
                var culture = new CultureInfo(result);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }

            await host.RunAsync();
        }
    }
}
