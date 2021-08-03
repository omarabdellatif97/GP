using DAL.Models;
using GP_API.FileEnvironments;
using GP_API.Services;
using GP_API.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]   // uncomment that attribute to secure the controller
    public class FileController : ControllerBase
    {
        //private readonly IFileServiceFactory fileServiceFactory;
        private readonly IFileService fileService;
        private readonly ICaseFileRepo fileRepo;
        private readonly ILogger<FileController> logger;

        public FileController(IFileService fileService,  ICaseFileRepo fileRepo, ILogger<FileController> logger /*,IFileServiceFactory fileServiceFactory */)
        {
            //this.fileServiceFactory = fileServiceFactory;
            this.fileService = fileService;
            this.fileRepo = fileRepo;
            this.logger = logger;

            // ftp://;19123.1.5/content/uploads/TempCaseFiles
            //var trashDirectoryService = this.fileServiceFactory.GetFileService(FileServiceMode.Remote, "temp");
            //trashDirectoryService.UploadFile(new byte[2], "asdfasd/asdfasdf.txt");
            //var tempCasefilesService = this.fileServiceFactory.GetFileService(FileServiceMode.Local);
        }



        [HttpPost("upload")]
        public async Task<IActionResult> uploadFiles(IFormFile file)
        {
            try
            {
                var ext = Path.GetExtension(file.FileName);
                var url = $"{Guid.NewGuid()}{ext}";//1234123412341234.txt
                var contentType = file.ContentType;
                using Stream stream = file.OpenReadStream();


                await fileService.UploadFileAsync(stream, url);
                
                
                var caseFile = new CaseFile()
                {
                    FileURL = url,
                    ContentType = contentType,
                    Extension = ext,
                    FileSize = stream.Length,
                    FileName = file.FileName
                };
                await fileRepo.Insert(caseFile);
                return Ok(MapURL(caseFile));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("download/{id}")]
        public async Task<IActionResult> downloadFile(int id)
        {
            try
            {
                var casefile = await fileRepo.GetById(id);
                if (casefile == null)
                    return NotFound(new { message = $"File not found with ID = {id}" });

                var file = await fileService.DownloadFileAsync(casefile.FileURL);

                return File(file, $"{casefile.ContentType}", casefile.FileName);
            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //[HttpGet("download")] // lcalhost/file/download/FileURL
        //public async Task<IActionResult> downloadFileWithUrl(string url)
        //{
        //    try
        //    {
        //        var casefile = await fileRepo.Get(url);
        //        if (casefile == null)
        //            return NotFound(new { message = $"File not found with ID = {url}" });

        //        var file = await fileService.DownloadFileAsync(casefile.FileURL);
        //        return Ok(File(file, $"{casefile.ContentType}"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> deleteFileWithUrl(int id)
        {
            try
            {
                var casefile = await fileRepo.GetById(id);
                if (casefile == null)
                    return NotFound(new { message = $"File not found with ID = {id}" });

                await fileService.DeleteFileAsync(casefile.FileURL);
                await fileRepo.Delete(casefile.Id);
                casefile = MapURL(casefile);
                return Ok(casefile);
            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("exists/{id}")]
        public async Task<IActionResult> IsFileExists(int id)
        {
            try
            {
                var casefile = await fileRepo.GetById(id);
                if (casefile == null)
                    return NotFound(new { message = $"File not found with ID = {id}" });

                await fileService.FileExistsAsync(casefile.FileURL);

                casefile = MapURL(casefile);
                return Ok(casefile);
            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        
        private CaseFile MapURL(CaseFile file)
        {
            file.URL = $@"{(Request.IsHttps ? @"https://" : @"http://")}{Request.Host.Value}/api/file/download/{file.Id}";
            return file;
        }
    }
}


// casefiles delete Action
// casefile post
// casefiles post
// 

// advanced
// ------------------------------------------------------------
// limit file size before action and controller creation.