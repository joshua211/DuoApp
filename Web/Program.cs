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

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMudServices();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddSingleton<IDuolingoClient, DuolingoClient>();
            builder.Services.AddSingleton<IValuePersistence, LocalPersistence>();
            builder.Services.AddSingleton<AuthenticationEntity>(new AuthenticationEntity()
            {
                DistinctId = "782244993",
                Timezone = "Europe/Berlin",
                FromLanguage = "en",
                LearningLanguage = "pt",
                LandingUrl = "https://www.duolingo.com/",
                InitialReferrer = "https://www.duolingo.com/learn",
                LastReferrer = "https://www.google.com/"
            });

            await builder.Build().RunAsync();
        }
    }
}
