using System;
using System.Threading.Tasks;
namespace Core.Interfaces
{
    public interface IValuePersistence
    {
        Task<(string value, DateTime storeDate)> GetValueAsync(string name);
        Task StoreValueAsync(string name, string value);
    }
}