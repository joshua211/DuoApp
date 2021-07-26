using System;
using System.Threading.Tasks;
namespace Core.Interfaces
{
    public interface IValuePersistence
    {
        Task<string> GetValueAsync(string name);
        Task StoreValueAsync(string name, string value);
        Task ClearAsync();
        Task ClearAsync(string name);
    }
}