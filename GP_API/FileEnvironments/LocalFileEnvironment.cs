using GP_API.Settings;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace GP_API.FileEnvironments
{
    public class LocalFileEnvironment : ILocalFileEnvironment
    {
        protected readonly IWebHostEnvironment env;
        protected readonly IFileServiceSettings settings;
        private string internalPath;

        public LocalFileEnvironment(IWebHostEnvironment env, IFileServiceSettings settings)
        {
            this.env = env;
            this.settings = settings;
        }

        /// <summary>
        /// the full path of wwwroot directory that changes depending on the location of the
        /// application, e.g D:\Projects\CSharpProjects\GP\GP_API\wwwroot\
        /// </summary>
        public virtual string AppRootPath => env.WebRootPath;

        /// <summary>
        /// the relative path of the application content directory,
        /// that is written in the appsettings.json in the key named RelativeContentPath,
        /// Notice that if there is Internal path specified in the creation of this LocalFileEnvironment
        /// it will be concatinated with that RelativeContentPath directly
        /// 
        /// </summary>
        public virtual string RelativeContentPath
        {
            get
            {
                if (internalPath != null)
                    return Path.Combine(settings.LocalServer.RelativeContentPath, internalPath);
                return settings.LocalServer.RelativeContentPath;
            }
        }

        /// <summary>
        /// it is the full path of the Application Content, that combined from the 
        /// wwwroot Directory, and the RelativeContentPath (including InternalPath if Exists)
        /// </summary>
        public virtual string FullContentPath => Path.Combine(AppRootPath, RelativeContentPath);


        //public ILocalServerSettings ServerSettings { get => this.settings; }




        /// <summary>
        /// this path is relative to the RelativeContentPath key in the appsettings.json file.
        /// it will be concatenated with it if it exists, and if it is null (not passed from UserInternalPath() Method)
        /// it will be ingored, and the RelativeContentPath will be used.
        /// Note that it can be setted only one time after that exception will be thrown.
        /// </summary>
        public virtual string InternalPath
        {
            get => internalPath;
            set
            {

                if (this.internalPath != null) throw new Exception("internal path can't be changed once it is setted.");
                this.internalPath = value;
            }
        }

        public virtual void UserInternalPath(string internalPath)
        {
            this.InternalPath = internalPath;
        }


        public virtual bool IsValidRelativePath(string relativePath)
        {
            if (relativePath == null 
                || relativePath == string.Empty 
                || settings.InternalPaths.ContainsValue(relativePath))
                return false;
            else
                return true;
        }

        public virtual string GetFullPath(string relativePath)
        {
            return Path.Combine(FullContentPath, relativePath);
        }

        public virtual string GetRelativeToAppRootPath(string relativePath)
        {
            return Path.Combine(RelativeContentPath, relativePath);
        }


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


