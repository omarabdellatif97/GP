namespace GP_API.Settings
{
    public interface IFileServiceSettings
    {
        LocalServerSettings LocalServer { get; }
        RemoteServerSettings RemoteServer { get; }
        FileServiceMode Mode { get; }
    }
}