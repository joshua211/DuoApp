using Newtonsoft.Json;

namespace Core.Entities
{
    public class Word
    {
        [JsonProperty("lexeme_id")]
        public string Id { get; set; }
        [JsonProperty("word")]
        public string Value { get; set; }
        [JsonProperty("translations")]
        public string Translation { get; set; }
        [JsonProperty("/dictionary/Portuguese/menina/3fdeaba20bb9699701746aba6d9d5fee")]
        public string Path { get; set; }
        [JsonProperty("pos")]
        public string WordType { get; set; }
        [JsonProperty("learning_language")]
        public string Language { get; set; }
    }
}