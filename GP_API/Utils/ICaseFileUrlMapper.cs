
using System.Collections.Generic;

namespace GP_API.Utils
{
    public interface ICaseFileUrlMapper
    {
        string DownloadActionUrl { get; set; }
        string RoutePattern { get; }
        string TemplateString { get; set; }

        List<string> ExtractFullUrls(string description);
        List<int> ExtractIds(string description);
        string GenerateDescription(string template);
        string GenerateTemplate(string description);
    }
}