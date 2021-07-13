using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
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

        [Function("Authenticate")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            HttpResponseData response;
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

                response = req.CreateResponse(result.StatusCode);
                return response;
            }

            if (result.Headers.TryGetValues("jwt", out var values))
            {
                response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync(values.First());

                return response;
            }

            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
