using GP_API.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.FileEnvironments
{
    public interface IFileEnvironmentFactory
    {
        IFileEnvironment GetFileEnvironment(FileServiceMode mode, string pathKey);
        IFileEnvironment GetFileEnvironment(FileServiceMode mode);
    }

    public class FileEnvironmentFactory : IFileEnvironmentFactory
    {
        private readonly Func<FileServiceMode,string,IFileEnvironment> getEnv;

        public FileEnvironmentFactory(Func<FileServiceMode,string,IFileEnvironment> getEnv)
        {
            this.getEnv = getEnv;
        }

        public IFileEnvironment GetFileEnvironment(FileServiceMode mode, string pathKey)
        {
            return this.getEnv(mode, pathKey);
        }
        public IFileEnvironment GetFileEnvironment(FileServiceMode mode)
        {
            return this.getEnv(mode, null);
        }
    }
}
