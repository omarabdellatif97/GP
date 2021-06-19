using FluentFTP;
using GP_API.FileEnvironments;
using GP_API.Services;
using GP_API.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;

namespace GP_API.UnitTest
{



    [TestClass]
    public class LocalFileServiceTests
    {

        private static LocalServerSettings settings = new LocalServerSettings()
        {
            RelativeContentPath = "knowledgebase",
        };

        private static ILocalFileEnvironment env;
        private static ILocalFileService service;

        

        private static TestContext context;

        [ClassInitialize]
        public static void Setup(TestContext ctx)
        {
        //    context = ctx;
        //    Mock<IWebHostEnvironment> mock = new Mock<IWebHostEnvironment>();
        //    mock.SetupGet(x => x.ContentRootPath).Returns(@"D:/Downloads/wwwroot");
        //    env = new LocalFileEnvironment(mock.Object,);
        //    service = new LocalFileService(env);
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            service = null;
            env = null;
            settings = null;
        }

        [TestMethod]
        public void UploadFile_ValidPath_FoundFileInServer()
        {
            // prepare
            var expected = true;
            
            
            //act
            var result = service.UploadFile(File.OpenRead(@"D:/Downloads/Temp/file1.txt"), @"app/file.txt");

            //assert
            Assert.Equals(result, expected);
        }
    }
}
