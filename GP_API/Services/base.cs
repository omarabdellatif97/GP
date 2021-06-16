using DAL.Models;
using FluentFTP;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace GP_API.Services
{


    public interface IFTPServerSettings
    {
        public string Uri { get; }
        public string Username { get; }
        public string Password { get; }
        public string AppRootPath { get;}
    }

    public class FTPServerSettings : IFTPServerSettings
    {
        public string Uri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AppRootPath { get; set; }
    }



    public interface IFileService: IDisposable
    {
        void DeleteDirectory(string remoteRelativePath);
        Task DeleteDirectoryAsync(string remoteRelativePath);
        void DeleteFile(string remoteRelativePath);
        Task DeleteFileAsync(string remoteRelativePath);
        bool DirectoryExists(string remoteRelativePath);
        Task<bool> DirectoryExistsAsync(string remoteRelativePath);
        byte[] DownloadFile(string remoteRelativePath);
        Task<byte[]> DownloadFileAsync(string remoteRelativePath);
        bool FileExists(string remoteRelativePath);
        Task<bool> FileExistsAsync(string remoteRelativePath);
        Stream OpenDownloadStream(string remoteRelativePath);
        Task<Stream> OpenDownloadStreamAsync(string remoteRelativePath);
        bool UploadFile(byte[] content, string remoteRelativePath);
        //string UploadFile(IFormFile formFile);
        bool UploadFile(Stream fileStream, string remoteRelativePath);
        Task<bool> UploadFileAsync(byte[] content, string remoteRelativePath);
        //Task<string> UploadFileAsync(IFormFile formFile);
        Task<bool> UploadFileAsync(Stream fileStream, string remoteRelativePath);

    }




    public class RemoteFileService : IFileService
    {
        // using FluentFtp;
        private FtpClient client;
        private readonly FTPServerSettings settings;
        //private readonly IRemotePath remotePath;
        private bool disposedValue;

        //public RemoteFileService(FtpClient ftpClient, FTPServerSettings settings, IRemotePath remotePath)
        //{
        //    this.client = ftpClient;
        //    this.settings = settings;
        //    this.remotePath = remotePath;
        //    this.client.Connect();
        //}

        public RemoteFileService(FtpClient ftpClient, FTPServerSettings settings)
        {
            this.client = ftpClient;
            this.settings = settings;
            this.client.Connect();
        }


        //public string UploadFile(IFormFile formFile)
        //{
        //    //var remoteFilePath = remotePath.NewRemotePath(formFile);
        //    var remoteFilePath = @$"{this.settings.}/{Guid.NewGuid()}.{Path.GetExtension(formFile.FileName)}";
        //    using var stream = formFile.OpenReadStream();
        //    FtpStatus status = client.Upload(stream, remoteFilePath);
        //    if (status != FtpStatus.Success) throw new FtpException("uploading of file failed.");
        //    return remoteFilePath;
        //}

        public bool UploadFile(Stream fileStream, string remoteRelativePath)
        {
            FtpStatus status = client.Upload(fileStream, remoteRelativePath);
            return status == FtpStatus.Success;
        }

        public bool UploadFile(byte[] content, string remoteRelativePath)
        {
            var status = client.Upload(content, remoteRelativePath);

            return status == FtpStatus.Success;
        }


        //public async Task<string> UploadFileAsync(IFormFile formFile)
        //{
        //    //var remoteFilePath = remotePath.NewRemotePath(formFile);
        //    var remoteFilePath = @$"{this.settings.ContentRootPath}/{Guid.NewGuid()}.{Path.GetExtension(formFile.FileName)}";
        //    using var stream = formFile.OpenReadStream();
        //    FtpStatus status = await client.UploadAsync(stream, remoteFilePath);
        //    if (status != FtpStatus.Success) throw new FtpException("uploading of file failed.");
        //    return remoteFilePath;
        //}

        public async Task<bool> UploadFileAsync(Stream fileStream, string remoteRelativePath)
        {
            FtpStatus status = await client.UploadAsync(fileStream, remoteRelativePath);
            return status == FtpStatus.Success;
        }

        public async Task<bool> UploadFileAsync(byte[] content, string remoteRelativePath)
        {
            var status = await client.UploadAsync(content, remoteRelativePath);

            return status == FtpStatus.Success;
        }

        public byte[] DownloadFile(string remoteRelativePath)
        {

            if (client.Download(out byte[] content, remoteRelativePath))
            {
                return content;
            }
            else
            {
                throw new FtpException($"download of the requested file: {remoteRelativePath} failed.");
            }
        }

        public async Task<byte[]> DownloadFileAsync(string remoteRelativePath)
        {
            byte[] data = await client.DownloadAsync(remoteRelativePath, 0);
            return data;
        }


        public Stream OpenDownloadStream(string remoteRelativePath)
        {

            return client.OpenRead(remoteRelativePath, FtpDataType.Binary);
        }

        public async Task<Stream> OpenDownloadStreamAsync(string remoteRelativePath)
        {
            return await client.OpenReadAsync(remoteRelativePath, FtpDataType.Binary);
        }




        public bool FileExists(string remoteRelativePath)
        {
            return client.FileExists(remoteRelativePath);
        }

        public bool DirectoryExists(string remoteRelativePath)
        {
            return client.DirectoryExists(remoteRelativePath);
        }



        public async Task<bool> FileExistsAsync(string remoteRelativePath)
        {
            return await client.FileExistsAsync(remoteRelativePath);
        }

        public async Task<bool> DirectoryExistsAsync(string remoteRelativePath)
        {
            return await client.DirectoryExistsAsync(remoteRelativePath);
        }

        public void DeleteFile(string remoteRelativePath)
        {
            client.DeleteFile(remoteRelativePath);
        }

        public void DeleteDirectory(string remoteRelativePath)
        {
            client.DeleteDirectory(remoteRelativePath);
        }

        public Task DeleteFileAsync(string remoteRelativePath)
        {
            return client.DeleteFileAsync(remoteRelativePath);
        }

        public Task DeleteDirectoryAsync(string remoteRelativePath)
        {
            return client.DeleteDirectoryAsync(remoteRelativePath);
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


    public interface IRemoteServerInfo
    {
        IFTPServerSettings ServerSettings { get; }
        string FullAppPath { get; }
        string RelativeAppPath { get; }
        string ServerUrl { get; }
        IRemoteResourceInfo NewRemotePath(string relativePath);
        IRemoteResourceInfo NewRemotePath();// generate new remote file name 
        IRemoteResourceInfo NewRemotePath(IFormFile formFile);
    }

    public class RemoteServerInfo : IRemoteServerInfo
    {
        private readonly IServiceProvider serviceProvider;

        public IFTPServerSettings ServerSettings { get; }

        public RemoteServerInfo(IFTPServerSettings settings , IServiceProvider serviceProvider)
        {
            this.ServerSettings = settings;
            this.serviceProvider = serviceProvider;
        }

        public string RemoteUrl { get => ServerSettings.Uri; }

        public string FullAppPath { get => @$"{ServerSettings.Uri}/{ServerSettings.AppRootPath}"; }
        
        public string RelativeAppPath { get => @$"{ServerSettings.AppRootPath}"; }

        public string ServerUrl => ServerSettings.Uri;

        public IRemoteResourceInfo NewRemotePath(string relativePath)
        {
            throw new NotImplementedException();
        }

        public IRemoteResourceInfo NewRemotePath()
        {
            throw new NotImplementedException();
        }

        public IRemoteResourceInfo NewRemotePath(IFormFile formFile)
        {
            throw new NotImplementedException();
        }
    }

    public interface IRemoteResourceInfo
    {
        /// <summary>
        /// path relative to the root path of the application in the AppRootPath of the 
        /// FTPServerSettings
        /// </summary>
        public string RelativePath { get; }

        /// <summary>
        /// path from after the ip of the ftp server, starts from the root specified in the 
        /// AppRootPath of the FTPServerSettings
        /// for example: the part of /app/mypath/file.txt in  ftp://192.168.1.3/app/mypath/file.txt
        /// </summary>
        public string RootPath { get; }

        /// <summary>
        /// all path from the ftp ip to the end of the resource path 
        /// for example: ftp://192.168.1.3/app/file.txt
        /// </summary>
        public string FullPath { get; set; }


    }

    public class RemoteResourceInfo : IRemoteResourceInfo
    {
        private readonly IRemoteServerInfo remotePath;
        private readonly string fileName;

        public RemoteResourceInfo(IRemoteServerInfo remotePath, string fileName)
        {
            this.fileName = fileName;
            this.remotePath = remotePath;
        }

        public IFTPServerSettings ServerSettings { get; }

        public string RelativePath => throw new NotImplementedException();

        public string RootPath => throw new NotImplementedException();

        public string FullPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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






}
