using System.Collections.Generic;

namespace Web.Services
{
    public class ObjectService
    {
        private Dictionary<string, object> dictionary;

        public ObjectService()
        {
            dictionary = new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get => dictionary.TryGetValue(key, out var res) ? res : null;
            set => dictionary[key] = value;
        }

        public T Get<T>(string key) where  T : class => this[key] as T;
    }
}