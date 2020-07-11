using Microsoft.Extensions.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Graph;
using msgraph_angular_dotnetcore.Models;
using System.Net.Http.Json;
using System.Threading;
using System;
using Microsoft.Graph.Core.Requests;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Azure.Identity;
using Azure.Core;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class ApplicationService
    {
        private readonly GraphServiceClient client;

        public ApplicationService(
            DefaultAzureCredential defaultAzureCredential,
            ILogger<ApplicationService> logger
            )
        {
            client = new GraphServiceClient(new DelegateAuthenticationProvider(async message =>
            {

                var at = await defaultAzureCredential.GetTokenAsync(new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" }));
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", at.Token);


                await Task.CompletedTask;
            }));
            this.logger = logger;
        }

        readonly HttpClient httpClient = new HttpClient();
        private readonly ILogger<ApplicationService> logger;

        internal async System.Collections.Generic.IAsyncEnumerable<Application> QuickGetAsync(string searchText)
        {
            var rm = client.Applications.Request(new[] { new QueryOption("$count", "true") }).GetHttpRequestMessage();
            await client.Applications.Client.AuthenticationProvider.AuthenticateRequestAsync(rm);
            rm.Headers.Add("consistencyLevel", "eventual");
            rm.RequestUri = new Uri(rm.RequestUri.ToString() + $@"&$search=""displayName:{searchText}""");

            logger.LogTrace(rm.RequestUri.ToString());

            var result = await httpClient.SendAsync(rm);
            //var result = await httpClient.SendAsync(rm);

            result.EnsureSuccessStatusCode();

            var responseHandler = new ResponseHandler(new Serializer());
            var result2 = await responseHandler.HandleResponse<GraphListResponse<Application>>(result);

            //var result2 = await result.Content.ReadFromJsonAsync<GraphListResponse<Application>>();
            foreach (var item in result2.Value)
            {
                yield return item;
            }
        }
    }

    public class GraphListResponse<T>
    {



        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }
        [JsonProperty("value")]
        public IEnumerable<T> Value { get; set; }
    }


}