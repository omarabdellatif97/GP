using DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


    //interface IFileClient
    //{
    //    bool UploadFile(IFormFile file);
    //    bool UploadFiles(IFormCollection files);
    //    bool DownloadFiles();
    //    bool DownloadFile();
    //}


    public interface IFileRead
    {
        Stream Read(CaseFile file);
        Stream ReadAsync(CaseFile file);
    }

    public interface IFileWrite
    {    //    bool UploadFile(IFormFile file);
         //bool UploadFiles(IFormCollection files);
        List<string> Write(IFormCollection files);
        List<string> WriteAsync(IFormCollection files);
    }



    public interface IFileService : IFileRead, IFileWrite
    {
        
    }

    class FileService : IFileService
    {
        private readonly IFTPServerSettings settings;

        public FileService(IFTPServerSettings settings)
        {
            this.settings = settings;
        }

        public Stream Read(CaseFile file)
        {
            throw new NotImplementedException();
        }

        public Stream ReadAsync(CaseFile file)
        {
            throw new NotImplementedException();
        }

        public List<string> Write(IFormCollection files)
        {
            throw new NotImplementedException();
        }

        public List<string> WriteAsync(IFormCollection files)
        {
            throw new NotImplementedException();
        }
    }




}
