using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Web.Services;
using Core.Interfaces;
using Core.Application;
using Blazored.LocalStorage;
using Core.Options;
using Web.Persistence;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(builder.Configuration.GetSection("clientOptions").Get<ClientOptions>());
            builder.Services.AddSingleton<ObjectService>();
            builder.Services.AddMudServices();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddSingleton<IDuolingoClient, DuolingoClient>();
            builder.Services.AddSingleton<IValuePersistence, LocalPersistence>();

            await builder.Build().RunAsync();
        }
    }
}
