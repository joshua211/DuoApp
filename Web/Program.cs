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
using Blazored.LocalStorage;
using Core.Interfaces;
using Core.Application;
using Core.Entities;
using Web.Persistence;
using Core.Options;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.Configure<ClientOptions>(options =>
            {
                options = builder.Configuration.GetSection("clientOptions").Get<ClientOptions>();
            });
            builder.Services.AddMudServices();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddSingleton<IDuolingoClient, DuolingoClient>();
            builder.Services.AddSingleton<IValuePersistence, LocalPersistence>();

            await builder.Build().RunAsync();
        }
    }
}
