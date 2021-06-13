using DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
        void Read();
        void ReadAsync();
    }

    public interface IFileWrite
    {    //    bool UploadFile(IFormFile file);
         //bool UploadFiles(IFormCollection files);
        List<CaseFile> Write(IFormCollection files);
        List<CaseFile> WriteAsync(IFormCollection files);
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

        public void Read()
        {
            throw new NotImplementedException();
        }

        public void ReadAsync()
        {
            throw new NotImplementedException();
        }

        public List<CaseFile> Write(IFormCollection files)
        {
            throw new NotImplementedException();
        }

        public List<CaseFile> WriteAsync(IFormCollection files)
        {
            throw new NotImplementedException();
        }
    }




}
