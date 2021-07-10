using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Core.Interfaces;
using Newtonsoft.Json;

namespace Console
{
    public class FilePersistence : IValuePersistence
    {
        private readonly string fileName = "_persistence.json";

        public async Task<string> GetValueAsync(string name)
        {
            var dic = await LoadStore();

            if (dic.TryGetValue(name, out var val))
                return val;
            else
                return null;
        }

        public async Task StoreValueAsync(string name, string value)
        {
            var dic = await LoadStore();
            dic[name] = value;

            var json = JsonConvert.SerializeObject(dic);
            await File.WriteAllTextAsync(fileName, json);
        }

        private async Task<Dictionary<string, string>> LoadStore()
        {
            if (File.Exists(fileName))
            {
                var json = await File.ReadAllTextAsync(fileName);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }

            return new Dictionary<string, string>();
        }
    }
}