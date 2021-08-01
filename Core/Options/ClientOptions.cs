using Newtonsoft.Json;

namespace Core.Options
{
    public class ClientOptions
    {
        public string BaseAddress { get; set; }
        public AuthenticationObject AuthObject { get; set; }

        public class AuthenticationObject
        {
            [JsonProperty("distinctId")]
            public string DistinctId { get; set; }
            [JsonProperty("timezone")]
            public string Timezone { get; set; }
            [JsonProperty("fromLanguage")]
            public string FromLanguage { get; set; }
            [JsonProperty("learningLanguage")]
            public string LearningLanguage { get; set; }
            [JsonProperty("landingUrl")]
            public string LandingUrl { get; set; }
            [JsonProperty("initialReferrer")]
            public string InitialReferrer { get; set; }
            [JsonProperty("lastReferrer")]
            public string LastReferrer { get; set; }

        }
    }
}