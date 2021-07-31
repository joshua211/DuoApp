using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Api
{
    public class Authenticate
    {
        private readonly HttpClient client;
        private readonly ILogger<GetSkills> logger;

        public Authenticate(HttpClient client, ILogger<GetSkills> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        [FunctionName("Authenticate")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            var json = await req.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject(json);

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(obj)),
                RequestUri = new Uri("https://www.duolingo.com/2017-06-30/users?fields=id")
            };

            var result = await client.SendAsync(request);
            if (!result.IsSuccessStatusCode)
            {
                return new StatusCodeResult((int)result.StatusCode);
            }

            if (result.Headers.TryGetValues("jwt", out var values))
            {
                return new ObjectResult(values.First());
            }

            return new BadRequestResult();
        }
    }
}
