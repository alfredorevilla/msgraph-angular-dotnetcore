using System.Collections.Generic;

namespace msgraph_angular_dotnetcore.Models
{
    public class ApplicationModel
    {
        public string DisplayName { get; set; }

        public string AppId { get; set; }

        public IEnumerable<string> ServicePrincipalsIds { get; set; }
    }
}