using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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

        public IAsyncEnumerable<dynamic> QuickGet([FromQuery] string searchText)
        {
            return applicationService.QuickGetAsync(searchText);
        }
    }
}