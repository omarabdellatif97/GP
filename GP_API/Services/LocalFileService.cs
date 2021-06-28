using DAL.Models;
using GP_API.FileEnvironments;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace GP_API.Services
{

    public class LocalFileService : ILocalFileService
    {
        private readonly ILocalFileEnvironment env;

        public LocalFileService(ILocalFileEnvironment env)
        {
            this.env = env;
        }

        public ILocalFileEnvironment Environment => this.env;

        public virtual void DeleteDirectory(string relativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");

            string fileFullPath = env.GetFullPath(relativePath);
            Directory.Delete(fileFullPath,true);
        }

        public virtual Task DeleteDirectoryAsync(string relativePath)
        {
            return Task.Run(() =>
            {
                DeleteDirectory(relativePath);
            });
        }

        public virtual void DeleteFile(string relativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");
            
            string fileFullPath = env.GetFullPath(relativePath);
            File.Delete(fileFullPath);
        }

        public virtual Task DeleteFileAsync(string relativePath)
        {
            return Task.Run(() => {
                DeleteFile(relativePath);
            });
        }

        public virtual bool DirectoryExists(string relativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");
            
            string fileFullPath = env.GetFullPath(relativePath);
            return Directory.Exists(fileFullPath);
        }

        public virtual Task<bool> DirectoryExistsAsync(string relativePath)
        {
            return Task.Run(() => {
                return DirectoryExists(relativePath);
            });
        }

        public virtual byte[] DownloadFile(string relativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");

            
            string fileFullPath = env.GetFullPath(relativePath);
            return File.ReadAllBytes(fileFullPath);
        }

        public virtual Task<byte[]> DownloadFileAsync(string relativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");


            
            string fileFullPath = env.GetFullPath(relativePath);
            return File.ReadAllBytesAsync(fileFullPath);
        }

        public virtual bool FileExists(string relativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");
            
            string fileFullPath = env.GetFullPath(relativePath);
            return File.Exists(fileFullPath);
        }

        public virtual Task<bool> FileExistsAsync(string relativePath)
        {
            return Task.Run(() =>
            {
                return FileExists(relativePath);

            });
        }

        public Task MoveFileAsync(string relativePath, string newRelativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");
            string fileFullPath = env.GetFullPath(relativePath);
            string newFileFullPath = env.GetFullPath(newRelativePath);

            return Task.Run(() =>
            {
                File.Move(fileFullPath, newFileFullPath);
            });
        }

        public virtual Stream OpenDownloadStream(string relativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");

            
            string fileFullPath = env.GetFullPath(relativePath);
            return new BufferedStream(File.OpenRead(fileFullPath));

        }

        public virtual Task<Stream> OpenDownloadStreamAsync(string relativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");

            return Task.Run(() =>
            {
                return OpenDownloadStream(relativePath);
            });
        }

        public virtual bool UploadFile(byte[] content, string relativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");

            
            string fileFullPath = env.GetFullPath(relativePath);
            FileInfo file = new FileInfo(fileFullPath);
            if (!file.Directory.Exists) file.Directory.Create();

            File.WriteAllBytes(fileFullPath, content);
            return true;
        }

        public virtual bool UploadFile(Stream fileStream, string relativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");


            string fileFullPath = env.GetFullPath(relativePath);
            FileInfo file = new FileInfo(fileFullPath);
            if (!file.Directory.Exists) file.Directory.Create();

            using BufferedStream stream = new BufferedStream(File.OpenWrite(fileFullPath));
            fileStream.CopyTo(stream);
            return true;
        }

        public virtual async Task<bool> UploadFileAsync(byte[] content, string relativePath)
        {

            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");


            string fileFullPath = env.GetFullPath(relativePath);
            FileInfo file = new FileInfo(fileFullPath);
            if (!file.Directory.Exists) file.Directory.Create();
            
            await File.WriteAllBytesAsync(fileFullPath, content);
            return true;
        }

        public virtual async Task<bool> UploadFileAsync(Stream fileStream, string relativePath)
        {
            if (!env.IsValidRelativePath(relativePath))
                throw new ArgumentException("not valid relative path.");


            string fileFullPath = env.GetFullPath(relativePath);
            FileInfo file = new FileInfo(fileFullPath);
            if (!file.Directory.Exists) file.Directory.Create();
            
            using BufferedStream stream = new BufferedStream(File.OpenWrite(fileFullPath));
            await fileStream.CopyToAsync(stream);
            return true;
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


