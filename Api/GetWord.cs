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
using System.Net.Http.Headers;
using System.Linq;

namespace Api
{
    public class GetWord
    {
        private readonly HttpClient client;
        private readonly ILogger<GetSkills> logger;

        public GetWord(HttpClient client, ILogger<GetSkills> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        [FunctionName("GetWord")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {

            if (!req.Query.TryGetValue("id", out var id))
            {
                logger.LogError("No lexeme id in query");

                return new BadRequestObjectResult("No lexeme id present in query");
            }

            if (!req.Headers.TryGetValue("Authorization", out var jwt))
            {
                logger.LogError("No Authorization header present");

                return new UnauthorizedResult();
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.duolingo.com/api/1/dictionary_page?lexeme_id={id}&use_cache=true&from_language_id=en");
            var token = jwt.First().Split(' ');
            request.Headers.Authorization = new AuthenticationHeaderValue(token[0], token[1]);

            var result = await client.SendAsync(request);
            if (!result.IsSuccessStatusCode)
            {
                logger.LogError($"{(int)result.StatusCode} Failed to fetch word: {result.ReasonPhrase}");

                return new StatusCodeResult((int)result.StatusCode);
            }

            var json = await result.Content.ReadAsStringAsync();
            return new OkObjectResult(json);
        }
    }
}
