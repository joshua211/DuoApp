using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Entities
{
    public class Skill
    {
        [JsonProperty("language_string")]
        public string LanguageString { get; set; }
        [JsonProperty("learned")]
        public bool Learned { get; private set; }
        [JsonProperty("known_lexemes")]
        public List<string> Lexemes { get; set; }
        [JsonProperty("title")]
        public string Name { get; set; }
        [JsonProperty("url_title")]
        public string UrlTitle { get; set; }
        [JsonProperty("mastered")]
        public bool Mastered { get; set; }
    }
}