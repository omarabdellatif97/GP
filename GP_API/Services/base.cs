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
        public string ContentRootPath { get;}
        string AppRootPath { get;}
    }

    public class FTPServerSettings : IFTPServerSettings
    {
        public string Uri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContentRootPath { get; set; }
        public string AppRootPath { get; set; }
    }



    public interface IFileService: IDisposable
    {
        void DeleteDirectory(string path);
        Task DeleteDirectoryAsync(string path);
        void DeleteFile(string path);
        Task DeleteFileAsync(string path);
        bool DirectoryExists(string path);
        Task<bool> DirectoryExistsAsync(string path);
        byte[] DownloadFile(string path);
        Task<byte[]> DownloadFileAsync(string path);
        bool FileExists(string path);
        Task<bool> FileExistsAsync(string path);
        Stream OpenDownloadStream(string path);
        Task<Stream> OpenDownloadStreamAsync(string path);
        bool UploadFile(byte[] content, string filePath);
        string UploadFile(IFormFile formFile);
        bool UploadFile(Stream fileStream, string filePath);
        Task<bool> UploadFileAsync(byte[] content, string filePath);
        Task<string> UploadFileAsync(IFormFile formFile);
        Task<bool> UploadFileAsync(Stream fileStream, string filePath);

    }




    public class RemoteFileService : IFileService, IDisposable
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


        public string UploadFile(IFormFile formFile)
        {
            //var remoteFilePath = remotePath.NewRemotePath(formFile);
            var remoteFilePath = @$"{this.settings.ContentRootPath}/{Guid.NewGuid()}.{Path.GetExtension(formFile.FileName)}";
            using var stream = formFile.OpenReadStream();
            FtpStatus status = client.Upload(stream, remoteFilePath);
            if (status != FtpStatus.Success) throw new FtpException("uploading of file failed.");
            return remoteFilePath;
        }

        public bool UploadFile(Stream fileStream, string filePath)
        {
            FtpStatus status = client.Upload(fileStream, filePath);
            return status == FtpStatus.Success;
        }

        public bool UploadFile(byte[] content, string filePath)
        {
            var status = client.Upload(content, filePath);

            return status == FtpStatus.Success;
        }


        public async Task<string> UploadFileAsync(IFormFile formFile)
        {
            //var remoteFilePath = remotePath.NewRemotePath(formFile);
            var remoteFilePath = @$"{this.settings.ContentRootPath}/{Guid.NewGuid()}.{Path.GetExtension(formFile.FileName)}";
            using var stream = formFile.OpenReadStream();
            FtpStatus status = await client.UploadAsync(stream, remoteFilePath);
            if (status != FtpStatus.Success) throw new FtpException("uploading of file failed.");
            return remoteFilePath;
        }

        public async Task<bool> UploadFileAsync(Stream fileStream, string filePath)
        {
            FtpStatus status = await client.UploadAsync(fileStream, filePath);
            return status == FtpStatus.Success;
        }

        public async Task<bool> UploadFileAsync(byte[] content, string filePath)
        {
            var status = await client.UploadAsync(content, filePath);

            return status == FtpStatus.Success;
        }

        public byte[] DownloadFile(string path)
        {

            if (client.Download(out byte[] content, path))
            {
                return content;
            }
            else
            {
                throw new FtpException($"download of the requested file: {path} failed.");
            }
        }

        public async Task<byte[]> DownloadFileAsync(string path)
        {
            byte[] data = await client.DownloadAsync(path, 0);
            return data;
        }


        public Stream OpenDownloadStream(string path)
        {

            return client.OpenRead(path, FtpDataType.Binary);
        }

        public async Task<Stream> OpenDownloadStreamAsync(string path)
        {
            return await client.OpenReadAsync(path, FtpDataType.Binary);
        }




        public bool FileExists(string path)
        {
            return client.FileExists(path);
        }

        public bool DirectoryExists(string path)
        {
            return client.DirectoryExists(path);
        }



        public async Task<bool> FileExistsAsync(string path)
        {
            return await client.FileExistsAsync(path);
        }

        public async Task<bool> DirectoryExistsAsync(string path)
        {
            return await client.DirectoryExistsAsync(path);
        }

        public void DeleteFile(string path)
        {
            client.DeleteFile(path);
        }

        public void DeleteDirectory(string path)
        {
            client.DeleteDirectory(path);
        }

        public Task DeleteFileAsync(string path)
        {
            return client.DeleteFileAsync(path);
        }

        public Task DeleteDirectoryAsync(string path)
        {
            return client.DeleteDirectoryAsync(path);
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


    public interface IRemotePath
    {
        IFTPServerSettings ServerSettings { get; }
        string FullAppPath { get; }
        string FullContentPath { get; }
        string RelativeAppPath { get; }
        string RelativeContentPath { get; }
        string RemoteUrl { get; }

        IRemoteFile NewRemoteDirectory(string directory);
        IRemoteFile NewRemoteFile(string filename);
        IRemoteFile NewRemoteFile();
        IRemoteFile NewRemotePath(IFormFile formFile);
    }

    public class RemotePath : IRemotePath
    {

        public IFTPServerSettings ServerSettings { get; }

        public RemotePath(IFTPServerSettings settings)
        {
            this.ServerSettings = settings;
        }

        public string RemoteUrl { get => ServerSettings.Uri; }

        public string FullContentPath { get => @$"{ServerSettings.Uri}/{ServerSettings.AppRootPath}/{ServerSettings.ContentRootPath}"; }

        public string FullAppPath { get => @$"{ServerSettings.Uri}/{ServerSettings.AppRootPath}"; }

        public string RelativeContentPath { get => @$"{ServerSettings.AppRootPath}/{ServerSettings.ContentRootPath}"; }

        public string RelativeAppPath { get => @$"{ServerSettings.AppRootPath}"; }

        public IRemoteFile NewRemoteDirectory(string directory)
        {
            throw new NotImplementedException();
        }

        public IRemoteFile NewRemoteFile(string filename)
        {
            throw new NotImplementedException();
        }

        public IRemoteFile NewRemotePath(IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public IRemoteFile NewRemoteFile()
        {
            throw new NotImplementedException();
        }


        //public IRemoteFile NewRemoteFile(string filename)
        //{
        //    return $@"{ServerSettings.ContentRootPath}/{filename}";
        //}

        //public IRemoteFile NewRemoteDirectory(string directory)
        //{
        //    return $@"{ServerSettings.ContentRootPath}/{directory}";
        //}


        //public IRemoteFile NewRemotePath(IFormFile formFile)
        //{
        //    return @$"{this.ServerSettings.ContentRootPath}/{Guid.NewGuid()}.{Path.GetExtension(formFile.FileName)}";
        //}
    }

    public interface IRemoteFile
    {
        public string FileName { get; }
        public string RelativeContentFileName { get; }
        public string RelativeRootFileName { get; }

        //public string FullContentFileName { get; }
        //public string FullAppFileName { get; }


    }

    public class RemoteFile : IRemoteFile
    {
        private readonly IRemotePath remotePath;
        private readonly string fileName;

        public RemoteFile(IRemotePath remotePath, string fileName)
        {
            this.fileName = fileName;
            this.remotePath = remotePath;
        }

        public IFTPServerSettings ServerSettings { get; }

        public string FileName { get => this.fileName; }
        public string RelativeContentFileName { get => @$"{remotePath.RelativeContentPath}/{this.fileName}"; }
        public string RelativeRootFileName { get => @$"{remotePath.RelativeAppPath}/{this.fileName}"; }
        
        //public string FullContentFileName { get => @$"{remotePath.FullContentPath}/{this.fileName}"; }
        //public string FullAppFileName { get => @$"{remotePath.FullAppPath}/{this.fileName}"; }





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
