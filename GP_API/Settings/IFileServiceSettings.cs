using System.Collections.Generic;

namespace GP_API.Settings
{
    public interface IFileServiceSettings
    {
        LocalServerSettings LocalServer { get; }
        RemoteServerSettings RemoteServer { get; }
        FileServiceMode Mode { get; }
        Dictionary<string, string> InternalPaths { get;  }


    }
}