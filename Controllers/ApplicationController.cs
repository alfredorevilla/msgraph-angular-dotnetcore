using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using msgraph_angular_dotnetcore.Models;
using Services;

namespace msgraph_angular_dotnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly Services.ApplicationService applicationService;

        public ApplicationController(ApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }

        public async IAsyncEnumerable<ApplicationModel> QuickGet([FromQuery] string searchText)
        {
            var result = applicationService.SearchAsync(searchText);

            await foreach (var item in result)
            {
                yield return new ApplicationModel
                {
                    AppId = item.AppId,
                    DisplayName = item.DisplayName,
                    ServicePrincipalsIds = item.ServicePrincipalsIds,
                    OwnersNames = item.OwnersNames
                };
            }
        }
    }
}