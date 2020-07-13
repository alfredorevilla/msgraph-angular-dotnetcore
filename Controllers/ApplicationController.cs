using System.Threading.Tasks;
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

        [HttpGet]
        public Task<ApplicationResponseModel> Search([FromQuery] string searchText)
        {
            return applicationService.SearchAsync(searchText);
        }
    }
}