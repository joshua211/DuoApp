using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Net.Http.Headers;

namespace Api
{
    public class GetSkills
    {
        private readonly HttpClient client;
        private readonly ILogger<GetSkills> logger;

        public GetSkills(HttpClient client, ILogger<GetSkills> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        [FunctionName("GetSkills")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            StringValues jwt;
            if (!req.Headers.TryGetValue("Authorization", out jwt))
            {
                logger.LogWarning("No jwt header present");

                return new UnauthorizedResult();
            }

            req.Query.TryGetValue("name", out var name);
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://www.duolingo.com/users/{name}")
            };
            request.Headers.Add("Authorization", jwt.First());

            var result = await client.SendAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                logger.LogError($"{result.StatusCode} Failed to fetch access token: {result.ReasonPhrase}");

                return new StatusCodeResult((int)result.StatusCode);
            }

            var json = await result.Content.ReadAsStringAsync();
            return new ObjectResult(json);
        }
    }
}
