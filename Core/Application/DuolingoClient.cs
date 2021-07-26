using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Application
{
    public class DuolingoClient : IDuolingoClient
    {
        private readonly HttpClient client;
        private readonly ILogger<IDuolingoClient> logger;
        private readonly ClientOptions options;
        private readonly IValuePersistence persistence;
        private string username;

        public DuolingoClient(IValuePersistence persistence, ClientOptions options)
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri("https://www.duolingo.com/"),
            };
            this.persistence = persistence;
            this.options = options;
        }

        public bool IsAuthenticated { get; private set; }

        public async Task<AuthenticationResult> Authenticate(string username)
        {
            try
            {
                var jwt = await persistence.GetValueAsync("jwt");
                if (jwt is null)
                    jwt = await GetValidJwtTokenAsync();

                var token = new JwtSecurityToken(jwt);
                if (DateTime.Now > token.ValidTo)
                    jwt = await GetValidJwtTokenAsync();
                await persistence.StoreValueAsync("jwt", jwt);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                IsAuthenticated = true;
                this.username = username;

                return new AuthenticationResult(true, username);
            }
            catch (Exception)
            {
                return new AuthenticationResult(false, null);
            }
        }

        private async Task<string> GetValidJwtTokenAsync()
        {
            var json = JsonConvert.SerializeObject(options.AuthObject);
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(json),
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:7071/api/Authenticate")
            };
            var result = await client.SendAsync(request);

            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<IEnumerable<Skill>> GetSkillsAsync() => await GetSkills();

        private async Task<IEnumerable<Skill>> GetSkills()
        {
            var valueTuple = await persistence.GetValueAsync("skills");
            string json = valueTuple;
                if (!string.IsNullOrEmpty(json))
                    return JsonConvert.DeserializeObject<List<Skill>>(json);
            

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost:7071/api/GetSkills?{username}")
            };
            try
            {
                var result = await client.SendAsync(request);
                result.EnsureSuccessStatusCode();

                json = await result.Content.ReadAsStringAsync();

                var userObject = JsonConvert.DeserializeObject<JObject>(json);
                var skills = ((JArray) userObject["language_data"]["pt"]["skills"]);

                await persistence.StoreValueAsync("skills", skills.ToString());
                return skills.ToObject<List<Skill>>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<Skill> GetSkillAsync(string name) => (await GetSkills()).First(s => s.Name == name);

        public async Task<Word> GetWordAsync(string id)
        {
            var json = await persistence.GetValueAsync(id);
            if (!string.IsNullOrEmpty(json))
                return JsonConvert.DeserializeObject<Word>(json);

            var result =
                await client.GetAsync($"/api/1/dictionary_page?lexeme_id={id}&use_cache=true&from_language_id=en");
            result.EnsureSuccessStatusCode();

            json = await result.Content.ReadAsStringAsync();
            var word = JsonConvert.DeserializeObject<Word>(json);

            await persistence.StoreValueAsync(id, JsonConvert.SerializeObject(word));
            return word;
        }
    }
}