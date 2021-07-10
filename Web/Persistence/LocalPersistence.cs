using System.Threading.Tasks;
using Blazored.LocalStorage;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Persistence
{
    public class LocalPersistence : IValuePersistence
    {
        private readonly IServiceScopeFactory factory;

        public LocalPersistence(IServiceScopeFactory factory)
        {
            this.factory = factory;
        }

        public async Task<string> GetValueAsync(string name)
        {
            using var scope = factory.CreateScope();
            var localStorage = scope.ServiceProvider.GetService<ILocalStorageService>();
            return await localStorage.GetItemAsStringAsync(name);
        }
        

        public async Task StoreValueAsync(string name, string value)
        {
            using var scope = factory.CreateScope();
            var localStorage = scope.ServiceProvider.GetService<ILocalStorageService>();
            await localStorage.SetItemAsStringAsync(name, value);
        }
    }
}