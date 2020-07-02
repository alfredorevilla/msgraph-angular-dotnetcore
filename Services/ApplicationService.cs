using System.Threading.Tasks;
using msgraph_angular_dotnetcore.Models;

namespace Services
{
    public class ApplicationService
    {
        public ApplicationService()
        {
        }

        internal async System.Collections.Generic.IAsyncEnumerable<ApplicationModel> QuickGetAsync(string searchText)
        {
            await Task.CompletedTask;
            yield return new ApplicationModel { AppId = "wqasdsaa", DisplayName = "sdasdas", ServicePrincipalsIds = new[] { "sdas" } };
        }
    }
}