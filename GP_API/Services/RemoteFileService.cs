using FluentFTP;
using GP_API.FileEnvironments;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GP_API.Services
{
    public class RemoteFileService : IRemoteFileService, IDisposable
    {
        // using FluentFtp;
        private FtpClient client;
        private readonly IRemoteFileEnvironment fileEnv;
        private bool disposedValue;

        public IRemoteFileEnvironment Environment => this.fileEnv;

        public RemoteFileService(FtpClient ftpClient, IRemoteFileEnvironment fileEnv)
        {
            this.client = ftpClient;
            this.fileEnv = fileEnv;
            this.client.Connect();
        }

        public virtual bool UploadFile(Stream fileStream, string relativePath)
        {
            if (!fileEnv.IsValidRelativePath(relativePath) || fileStream == null)
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            FtpStatus status = client.Upload(fileStream, relativeAppPath,createRemoteDir:true);
            return status == FtpStatus.Success || status == FtpStatus.Skipped;
        }

        public virtual bool UploadFile(byte[] content, string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath) || content == null)
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            var status = client.Upload(content, relativeAppPath,createRemoteDir: true);

            return status == FtpStatus.Success || status == FtpStatus.Skipped;
        }


        //public virtual async Task<string> UploadFileAsync(IFormFile formFile)
        //{
        //    //var remoteFilePath = remotePath.NewRemotePath(formFile);
        //    var remoteFilePath = @$"{this.settings.ContentRootPath}/{Guid.NewGuid()}.{Path.GetExtension(formFile.FileName)}";
        //    using var stream = formFile.OpenReadStream();
        //    FtpStatus status = await client.UploadAsync(stream, remoteFilePath);
        //    if (status != FtpStatus.Success) throw new FtpException("uploading of file failed.");
        //    return remoteFilePath;
        //}

        public virtual async Task<bool> UploadFileAsync(Stream fileStream, string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath) || fileStream == null)
                throw new ArgumentException("invalid arguments.");

            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            FtpStatus status = await client.UploadAsync(fileStream, relativeAppPath, createRemoteDir: true);
            return status == FtpStatus.Success || status == FtpStatus.Skipped;
        }

        public virtual async Task<bool> UploadFileAsync(byte[] content, string relativePath)
        {

            
            if (!fileEnv.IsValidRelativePath(relativePath) || content == null)
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            var status = await client.UploadAsync(content, relativeAppPath,createRemoteDir: true);

            return status == FtpStatus.Success || status == FtpStatus.Skipped;
        }

        public virtual byte[] DownloadFile(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            //if (!client.FileExists(relativeAppPath)) throw new FileNotFoundException("the requested file is not exists in the remote server");

            try
            {
                bool result = client.Download(out byte[] content, relativeAppPath);
                if (!result) throw new FtpException("download failed exception");
                return content;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual async Task<byte[]> DownloadFileAsync(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            //if (!(await  client.FileExistsAsync(relativeAppPath))) throw new FileNotFoundException("the requested file is not exists in the remote server");
            try
            {
                byte[] data = await client.DownloadAsync(relativeAppPath, 0);
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public virtual Stream OpenDownloadStream(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            return client.OpenRead(relativeAppPath, FtpDataType.Binary);
        }

        public virtual async Task<Stream> OpenDownloadStreamAsync(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            return await client.OpenReadAsync(relativeAppPath, FtpDataType.Binary);
        }




        public virtual bool FileExists(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            return client.FileExists(relativeAppPath);
        }

        public virtual bool DirectoryExists(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            return client.DirectoryExists(relativeAppPath);
        }



        public virtual async Task<bool> FileExistsAsync(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            return await client.FileExistsAsync(relativeAppPath);
        }

        public virtual async Task<bool> DirectoryExistsAsync(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            return await client.DirectoryExistsAsync(relativeAppPath);
        }

        public virtual void DeleteFile(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            client.DeleteFile(relativeAppPath);
        }

        public virtual void DeleteDirectory(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            client.DeleteDirectory(relativeAppPath);
        }

        public virtual Task DeleteFileAsync(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            return client.DeleteFileAsync(relativeAppPath);
        }

        public virtual Task DeleteDirectoryAsync(string relativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            return client.DeleteDirectoryAsync(relativeAppPath);
        }


        //MoveFileAsync

        public virtual Task MoveFileAsync(string relativePath,string newRelativePath)
        {

            if (!fileEnv.IsValidRelativePath(relativePath))
                throw new ArgumentException("invalid arguments.");

            if (!fileEnv.IsValidRelativePath(newRelativePath))
                throw new ArgumentException("invalid arguments.");
            string relativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            string newRelativeAppPath = fileEnv.GetRelativeToAppRootPath(relativePath);
            client.CreateDirectory(newRelativeAppPath);
            return client.MoveFileAsync(relativeAppPath, newRelativeAppPath,
                existsMode:FtpRemoteExists.Overwrite);
        }


        #region disposing
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                this.client.Disconnect();
                this.client = null;
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RemoteFileService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }















    // -----------------------------------------------------------------------------------
    // old





    //public interface IFileRead
    //{
    //    //Stream Read(CaseFile file);
    //    //Task<Stream> ReadAsync(CaseFile file);
    //}

    //public interface IFileWrite
    //{   
    ////    List<string> Write(IFormCollection files);
    ////    Task<List<string>> WriteAsync(IFormCollection files);
    //}




    //public class FileService : IFileService
    //{
    //    private readonly IFTPFileClient client;

    //    public FileService(IFTPFileClient client)
    //    {
    //        this.client = client;
    //    }

    //    public Stream Read(CaseFile file)
    //    {
    //        return client.DownloadFile(file.FileURL); 
    //    }

    //    public Task<Stream> ReadAsync(CaseFile file)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public List<string> Write(IFormCollection files)
    //    {
    //        List<string> urls = new List<string>();

    //        // root of the case
    //        //var dir = Path.Combine("Case", Guid.NewGuid().ToString());
    //        //client.CreateDirectory(dir);

    //        foreach (var item in files.Files)
    //        {
    //            var extension = Path.GetExtension(item.FileName);
    //            //var fileDir = Path.Combine(dir, $"{Guid.NewGuid()}.{extension}");
    //            var fileDir = Path.Combine($"casefile-{Guid.NewGuid()}.{extension}");
    //            client.UploadFile(item.OpenReadStream(), fileDir);

    //            urls.Add(fileDir);
    //        }

    //        return urls;
    //    }

    //    public Task<List<string>> WriteAsync(IFormCollection files)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    // call this function like Prepare(WebRequestMethods.Ftp.UploadFile);



    //}




    //public interface IFTPFileClient
    //{
    //    public IFTPServerSettings Settings { get; }
    //    string UploadFile(Stream stream, string path);
    //    Stream DownloadFile(string path);
    //    bool CreateDirectory(string path);
    //    bool DeleteDirectory(string path);
    //    bool DeleteFile(string path);

    //}

    //public class FTPFileClient : IFTPFileClient
    //{

    //    public FTPFileClient(IFTPServerSettings settings)
    //    {
    //        Settings = settings;
    //    }
    //    public IFTPServerSettings Settings { get; }

    //    public bool CreateDirectory(string path)
    //    {
    //        if (path == null)
    //            throw new ArgumentException("path can't be null");

    //        WebRequest request = PrepareRequest(WebRequestMethods.Ftp.MakeDirectory, path);
    //        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
    //        {
    //            return response.StatusCode == FtpStatusCode.CommandOK;
    //        }
    //    }

    //    public bool DeleteDirectory(string path)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool DeleteFile(string path)
    //    {
    //        if (path == null)
    //            throw new ArgumentException("path can't be null");

    //        WebRequest request = PrepareRequest(WebRequestMethods.Ftp.UploadFile, path);
    //        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
    //        {
    //            return response.StatusCode == FtpStatusCode.CommandOK;
    //        }


    //    }

    //    public Stream DownloadFile(string path)
    //    {

    //        if (path == null)
    //            throw new ArgumentException("path can't be null");

    //        WebRequest request = PrepareRequest(WebRequestMethods.Ftp.UploadFile, path);
    //        return request.GetResponse().GetResponseStream();
    //    }

    //    public string UploadFile(Stream stream,string path)
    //    {
    //        if (stream == null)
    //            throw new ArgumentException("stream can't be null");
    //        if(path == null)
    //            throw new ArgumentException("path can't be null");
    //        try
    //        {

    //            WebRequest request = PrepareRequest(WebRequestMethods.Ftp.UploadFile, path);
    //            using (Stream ftpStream = request.GetRequestStream())
    //            {
    //                stream.CopyTo(ftpStream);
    //            }
    //            return path;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            Console.WriteLine(ex.StackTrace);
    //            throw;
    //        }
    //    }

    //    private WebRequest PrepareRequest(string method, string path)
    //    {
    //        if (path == null)
    //            throw new ArgumentException("path can't be null");
    //        if (!IsValidFtpMethod(method))
    //            throw new ArgumentException("not valid string for ftp method");
    //        var realPath = @$"{Settings.Uri}/{path}";
    //        FtpWebRequest request =
    //            (FtpWebRequest)WebRequest.Create(realPath);
    //        request.Credentials = new NetworkCredential(Settings.Username, Settings.Password);
    //        request.KeepAlive = false;
    //        request.UseBinary = true;
    //        //request.ContentType = "application/unknown";
    //        request.Method = method;
    //        return request;
    //    }


    //    private bool IsValidFtpMethod(string method)
    //    {
    //        var str =
    //            new string[]{ "APPE", "DELE", "RETR", "MDTM", "SIZE",
    //                "NLST", "LIST", "MKD", "PWD", "RMD", "RENAME", "STOR", "STOU" };
    //        return str.Any(s => string.Equals(s, method, StringComparison.Ordinal));

    //    }

    //}




    //public interface IRemoteServerInfo
    //{
    //    IFtpServerSettings ServerSettings { get; }
    //    string FullAppPath { get; }
    //    string RelativeAppPath { get; }
    //    string ServerUrl { get; }
    //    IRemoteResourceInfo NewRemotePath(string relativePath);
    //    IRemoteResourceInfo NewRemotePath();// generate new remote file name 
    //    IRemoteResourceInfo NewRemotePath(IFormFile formFile);
    //}

    //public class RemoteServerInfo : IRemoteServerInfo
    //{
    //    private readonly IServiceProvider serviceProvider;

    //    public IFtpServerSettings ServerSettings { get; }

    //    public RemoteServerInfo(IFtpServerSettings settings, IServiceProvider serviceProvider)
    //    {
    //        this.ServerSettings = settings;
    //        this.serviceProvider = serviceProvider;
    //    }

    //    public string RemoteUrl { get => ServerSettings.Uri; }

    //    public string FullAppPath { get => @$"{ServerSettings.Uri}/{ServerSettings.RelativeAppPath}"; }

    //    public string RelativeAppPath { get => @$"{ServerSettings.RelativeAppPath}"; }

    //    public string ServerUrl => ServerSettings.Uri;

    //    public IRemoteResourceInfo NewRemotePath(string relativePath)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IRemoteResourceInfo NewRemotePath()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IRemoteResourceInfo NewRemotePath(IFormFile formFile)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public interface IRemoteResourceInfo
    //{
    //    /// <summary>
    //    /// path relative to the root path of the application in the AppRootPath of the 
    //    /// FTPServerSettings
    //    /// </summary>
    //    public string RelativePath { get; }

    //    /// <summary>
    //    /// path from after the ip of the ftp server, starts from the root specified in the 
    //    /// AppRootPath of the FTPServerSettings
    //    /// for example: the part of /app/mypath/file.txt in  ftp://192.168.1.3/app/mypath/file.txt
    //    /// </summary>
    //    public string RootPath { get; }

    //    /// <summary>
    //    /// all path from the ftp ip to the end of the resource path 
    //    /// for example: ftp://192.168.1.3/app/file.txt
    //    /// </summary>
    //    public string FullPath { get; set; }


    //}

    //public class RemoteResourceInfo : IRemoteResourceInfo
    //{
    //    private readonly IRemoteServerInfo remotePath;
    //    private readonly string fileName;

    //    public RemoteResourceInfo(IRemoteServerInfo remotePath, string fileName)
    //    {
    //        this.fileName = fileName;
    //        this.remotePath = remotePath;
    //    }

    //    public IFTPServerSettings ServerSettings { get; }

    //    public string RelativePath => throw new NotImplementedException();

    //    public string RootPath => throw new NotImplementedException();

    //    public string FullPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    //}






}


