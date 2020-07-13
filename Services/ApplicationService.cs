using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using msgraph_angular_dotnetcore.Models;
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

        public async IAsyncEnumerable<ApplicationModel> SearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                throw new ArgumentException(nameof(searchText));
            }

            var applicationPage = await client.Applications.Request(new Option[] { new HeaderOption("consistencyLevel", "eventual"), new QueryOption("$count", "true") }).Top(5).GetAsync();

            var batchRequestContent = new BatchRequestContent();

            var servicePrincipalsRequestId = batchRequestContent.AddBatchRequestStep(
            client.ServicePrincipals.Request().Select("id").Filter(string.Join(" or ", applicationPage.Select(a => $"appId eq '{a.AppId}'"))));

            var ownerRequestIds = applicationPage.Select(a => new { a.AppId, Id = batchRequestContent.AddBatchRequestStep(client.Applications[a.Id].Owners.Request().Select("id,displayName,userPrincipalName")) }).ToDictionary(a => a.AppId, a => a.Id);

            var returnedResponse = await client.Batch.Request().PostAsync(batchRequestContent);

            var servicePrincipalsPage = await returnedResponse.GetResponseByIdAsync<GraphServiceServicePrincipalsCollectionResponse>(servicePrincipalsRequestId);

            foreach (var item in applicationPage)
            {
                var owners = (await returnedResponse.GetResponseByIdAsync<ApplicationOwnersCollectionWithReferencesResponse>(ownerRequestIds[item.AppId])).Value;

                var servicePrincipals = servicePrincipalsPage.Value
                    .Where(p => p.AppId == item.AppId);

                yield return new ApplicationModel
                {
                    AppId = item.AppId,
                    DisplayName = item.DisplayName,
                    ServicePrincipalsIds = servicePrincipals.Select(p => p.Id).ToArray(),
                    OwnersNames = owners.Cast<dynamic>().Select(d => (string)d.DisplayName).ToArray()
                };
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