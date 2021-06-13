using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Services
{
     interface IFTPServerSettings
    {
        public string Uri { get; }
        public string Username { get; }
        public string Password { get; }
    }
    
    interface IFileClient
    {
        bool UploadFile(IFormFile file);
        bool UploadFiles(IFormCollection files);
        bool DownloadFiles();
        bool DownloadFile();
    }


     interface IFileRead
    {
        public IFTPServerSettings Client { get; }
        void Read();
        void ReadAsync();
    }

     interface IFileWrite
    {
        public IFTPServerSettings Client { get; }
        void Write();
        void WriteAsync();
    }



     interface IFileService : IFileRead, IFileWrite
    {
        
    }




}
