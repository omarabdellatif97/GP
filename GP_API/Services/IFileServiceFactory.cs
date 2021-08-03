using GP_API.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Services
{
   
    public interface IFileServiceFactory
    {
        IFileService GetFileService(FileServiceMode mode, string pathKey);
        IFileService GetFileService(FileServiceMode mode);

    }

    public class FileServiceFactory : IFileServiceFactory
    {
        private readonly Func<FileServiceMode,string,IFileService> getService;

        public FileServiceFactory(Func<FileServiceMode,string,IFileService> getService)
        {
            this.getService = getService;
        }

        public IFileService GetFileService(FileServiceMode mode,string pathKey)
        {
            return this.getService(mode,pathKey);
        }

        public IFileService GetFileService(FileServiceMode mode)
        {
            return this.getService(mode, null);
        }
    }


}
