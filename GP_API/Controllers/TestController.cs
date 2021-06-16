using GP_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Logging;

namespace GP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private List<string> paths = new List<string>()
        {
            @"D:/Downloads/Temp/file1.txt",
            @"D:/Downloads/Temp/image.jpg",
            @"D:/Downloads/Temp/Moamen Soroor CV.pdf",

        };
        private readonly ILogger<TestController> logger;
        private readonly IFileService service;
        private readonly IFileEnvironment env;

        public TestController (ILogger<TestController> logger ,IFileService service, IFileEnvironment env)
        {
            this.logger = logger;
            this.service = service;
            this.env = env;
        }


        [HttpGet]
        [Route("upload/{id}")]
        public IActionResult TestUploadFiles(int id)
        {
            try
            {

                using FileStream fileStream = System.IO.File.OpenRead(paths[id]);
                bool result = service.UploadFile(fileStream, $"{Guid.NewGuid()}.{Path.GetExtension(paths[id])}");
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        [HttpGet]
        [Route("download/{id}")]
        public IActionResult TestDownloadFiles(string id)
        {
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }




    }
}
