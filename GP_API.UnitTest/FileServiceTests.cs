using FluentFTP;
using GP_API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;

namespace GP_API.UnitTest
{
    [TestClass]
    public class FileServiceTests
    {


        [TestMethod]
        public void UploadFile_ValidPath_FoundFileInServer()
        {
            // prepare
            var settings = new FTPServerSettings()
            {
                Uri = "ftp://192.168.1.3",
                Username = "FtpServer",
                Password = "123456",
                AppRootPath = "knowledgebase",
                ContentRootPath="wwwroot"
            };
            FtpClient ftp = new FtpClient(settings.Uri);
            var service = new RemoteFileService(ftp,settings);
            var expected = @"app/file.txt";
            
            
            //act
            var result = service.UploadFile(File.OpenRead(@"D:/Downloads/Temp/file1.txt"), @"app/file.txt");

            //assert
            StringAssert.Equals(result, expected);
        }
    }
}
