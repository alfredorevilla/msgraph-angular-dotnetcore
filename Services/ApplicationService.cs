using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Newtonsoft.Json;

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

        private readonly ILogger<ApplicationService> logger;

        public async IAsyncEnumerable<Application> SearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                throw new ArgumentException(nameof(searchText));
            }

            var rm = await client.Applications.Request(new Option[] { new HeaderOption("consistencyLevel", "eventual"), new QueryOption("$count", "true") }).GetAsync();

            foreach (var item in rm)
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