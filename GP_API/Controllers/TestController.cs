using GP_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using GP_API.FileEnvironments;

namespace GP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private List<string> paths = new List<string>()
        {
            @"temp/file1.txt",
            @"temp/image.jpg",
            @"temp/00cb4c64-e8ae-4eb5-aa23-145507fd6913.jpg",

        };
        private readonly ILogger<TestController> logger;
        private readonly IWebHostEnvironment env;
        private readonly IFileService service;
        private readonly IFileEnvironment fileEnv;

        public TestController (ILogger<TestController> logger,IWebHostEnvironment env ,IFileService service, IFileEnvironment fileEnv)
        {
            this.logger = logger;
            this.env = env;
            this.service = service;
            this.fileEnv = fileEnv;
        }

        // async starts here
        //------------------------------------------------------------------------------

        [HttpGet]
        [Route("upload/{id}")]
        public IActionResult TestUploadFiles(int id)
        
        {
            try
            {

                using FileStream fileStream = System.IO.File.OpenRead(Path.Combine(env.WebRootPath, paths[id]));
                bool result = service.UploadFile(fileStream, $"{Path.GetFileName(paths[id])}");
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        //[HttpGet]
        //[Route("upload/{id}")]
        //public IActionResult TestUploadFiles(IFormFile file)

        //{
        //    try
        //    {
        //        Stream stream = file.OpenReadStream();

        //        var extension = Path.GetExtension(file.FileName);

        //        using FileStream fileStream = System.IO.File.OpenRead(paths[id]);
        //        bool result = service.UploadFile(fileStream, $"{Path.GetFileName(paths[id])}");

        //        bool result = service.UploadFile(fileStream, $"{Guid.NewGuid()}.{Path.GetExtension(File.FileName)}");
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest();
        //    }
        //}


        [HttpGet]
        [Route("download/{id}")]
        public IActionResult TestDownloadFiles(int id)
        {
            try
            {
                // fileUrl

                var file = service.DownloadFile(Path.GetFileName(paths[id]));
                return File(file, "application/unknown");
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult TestDeleteFile(int id)
        {
            try
            {
                // fileUrl

                service.DeleteFile(Path.GetFileName(paths[id]));
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        [HttpGet]
        [Route("fileexists/{id}")]
        public IActionResult TestFileExists(int id)
        {
            try
            {
                // fileUrl

                return Ok(service.FileExists(Path.GetFileName(paths[id])));
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        [HttpGet]
        [Route("direxists/{id}")]
        public IActionResult TestDirectoryExists(int id)
        {
            try
            {
                // fileUrl

                return Ok(service.FileExists(Path.GetFileName(paths[id])));
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        // async starts here
        //------------------------------------------------------------------------------


        [HttpGet]
        [Route("asyncupload/{id}")]
        public async Task<IActionResult> TestUploadFilesAsync(int id)

        {
            try
            {

                using FileStream fileStream = System.IO.File.OpenRead(Path.Combine(env.WebRootPath, paths[id]));
                bool result = await  service.UploadFileAsync(fileStream, $"{Path.GetFileName(paths[id])}");
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        //[HttpGet]
        //[Route("upload/{id}")]
        //public IActionResult TestUploadFiles(IFormFile file)

        //{
        //    try
        //    {
        //        Stream stream = file.OpenReadStream();

        //        var extension = Path.GetExtension(file.FileName);

        //        using FileStream fileStream = System.IO.File.OpenRead(paths[id]);
        //        bool result = service.UploadFile(fileStream, $"{Path.GetFileName(paths[id])}");

        //        bool result = service.UploadFile(fileStream, $"{Guid.NewGuid()}.{Path.GetExtension(File.FileName)}");
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest();
        //    }
        //}


        [HttpGet]
        [Route("asyncdownload/{id}")]
        public async Task<IActionResult> TestDownloadFilesAsync(int id)
        {
            try
            {
                // fileUrl

                var file = await service.DownloadFileAsync(Path.GetFileName(paths[id]));
                return File(file, "application/unknown");
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        [HttpGet]
        [Route("asyncdelete/{id}")]
        public async Task<IActionResult> TestDeleteFileAsync(int id)
        {
            try
            {
                // fileUrl

                await service.DeleteFileAsync(Path.GetFileName(paths[id]));
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        [HttpGet]
        [Route("asyncfileexists/{id}")]
        public async Task<IActionResult> TestFileExistsAsync(int id)
        {
            try
            {
                // fileUrl

                return Ok(await service.FileExistsAsync(Path.GetFileName(paths[id])));
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        [HttpGet]
        [Route("asyncdirexists/{id}")]
        public async Task<IActionResult> TestDirectoryExistsAsync(int id)
        {
            try
            {
                // fileUrl

                return Ok(await service.FileExistsAsync(Path.GetFileName(paths[id])));
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }



    }
}
