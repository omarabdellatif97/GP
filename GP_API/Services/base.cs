using DAL.Models;
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
    }
    public class FTPServerSettings : IFTPServerSettings
    {
        public string Uri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }



    public interface IFileRead
    {
        Stream Read(CaseFile file);
        Task<Stream> ReadAsync(CaseFile file);
    }

    public interface IFileWrite
    {   
        List<string> Write(IFormCollection files);
        Task<List<string>> WriteAsync(IFormCollection files);
    }



    public interface IFileService : IFileRead, IFileWrite
    {
        //bool Delete(CaseFile mycase);

    }

    public class FileService : IFileService
    {
        private readonly IFTPFileClient client;

        public FileService(IFTPFileClient client)
        {
            this.client = client;
        }

        public Stream Read(CaseFile file)
        {
            return client.DownloadFile(file.FileURL); 
        }

        public Task<Stream> ReadAsync(CaseFile file)
        {
            throw new NotImplementedException();
        }

        public List<string> Write(IFormCollection files)
        {
            List<string> urls = new List<string>();
            
            // root of the case
            var dir = Path.Combine("Case", Guid.NewGuid().ToString());
            client.CreateDirectory(dir);

            foreach (var item in files.Files)
            {
                var extension = Path.GetExtension(item.FileName);
                var fileDir = Path.Combine(dir, $"{Guid.NewGuid()}.{extension}");
                client.UploadFile(item.OpenReadStream(), fileDir);
                
                urls.Add(fileDir);
            }

            return urls;
        }

        public Task<List<string>> WriteAsync(IFormCollection files)
        {
            throw new NotImplementedException();
        }

        // call this function like Prepare(WebRequestMethods.Ftp.UploadFile);



    }




    public interface IFTPFileClient
    {
        public IFTPServerSettings Settings { get; }
        string UploadFile(Stream stream, string path);
        Stream DownloadFile(string path);
        bool CreateDirectory(string path);
        bool DeleteDirectory(string path);
        bool DeleteFile(string path);

    }

    public class FTPFileClient : IFTPFileClient
    {

        public FTPFileClient(IFTPServerSettings settings)
        {
            Settings = settings;
        }
        public IFTPServerSettings Settings { get; }

        public bool CreateDirectory(string path)
        {
            if (path == null)
                throw new ArgumentException("path can't be null");

            WebRequest request = PrepareRequest(WebRequestMethods.Ftp.MakeDirectory, path);
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.StatusCode == FtpStatusCode.CommandOK;
            }
        }

        public bool DeleteDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFile(string path)
        {
            if (path == null)
                throw new ArgumentException("path can't be null");

            WebRequest request = PrepareRequest(WebRequestMethods.Ftp.UploadFile, path);
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.StatusCode == FtpStatusCode.CommandOK;
            }


        }

        public Stream DownloadFile(string path)
        {

            if (path == null)
                throw new ArgumentException("path can't be null");

            WebRequest request = PrepareRequest(WebRequestMethods.Ftp.UploadFile, path);
            return request.GetResponse().GetResponseStream();
        }

        public string UploadFile(Stream stream,string path)
        {
            if (stream == null)
                throw new ArgumentException("stream can't be null");
            if(path == null)
                throw new ArgumentException("path can't be null");

            WebRequest request = PrepareRequest(WebRequestMethods.Ftp.UploadFile,path);
            using (Stream ftpStream = request.GetRequestStream())
            {
                stream.CopyTo(ftpStream);
            }
            return path;
        }

        private WebRequest PrepareRequest(string method, string path)
        {
            if (path == null)
                throw new ArgumentException("path can't be null");
            if (!IsValidFtpMethod(method))
                throw new ArgumentException("not valid string for ftp method");
            FtpWebRequest request =
                (FtpWebRequest)WebRequest.Create(Path.Combine(Settings.Uri,path));
            request.Credentials = new NetworkCredential(Settings.Username, Settings.Password);
            request.Method = method;
            return request;
        }


        private bool IsValidFtpMethod(string method)
        {
            var str =
                new string[]{ "APPE", "DELE", "RETR", "MDTM", "SIZE",
                    "NLST", "LIST", "MKD", "PWD", "RMD", "RENAME", "STOR", "STOU" };
            return str.Any(s => string.Equals(s, method, StringComparison.Ordinal));

        }

    }






}
