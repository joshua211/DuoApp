using System.Runtime.CompilerServices;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
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

        [Function("GetSkills")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            HttpResponseData response;
            var jwt = req.Headers.GetValues("Authorization").FirstOrDefault();
            if (jwt is null)
            {
                response = req.CreateResponse(HttpStatusCode.Unauthorized);
                logger.LogWarning("No jwt header present");

                return response;
            }

            string name = req.Url.Query.Trim('?');
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://www.duolingo.com/users/{name}")
            };
            request.Headers.Add("Authorization", jwt);

            var result = await client.SendAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                response = req.CreateResponse();
                response.StatusCode = result.StatusCode;

                return response;
            }

            var jsonStream = await result.Content.ReadAsStreamAsync();
            response = req.CreateResponse(HttpStatusCode.OK);
            response.Body = jsonStream;
            response.Headers.Add("Content-Type", "application/json");

            return response;
        }
    }
}
