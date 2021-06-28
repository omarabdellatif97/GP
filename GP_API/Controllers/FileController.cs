using DAL.Models;
using GP_API.FileEnvironments;
using GP_API.Services;
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
    //[Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileService fileService;
        private readonly CaseContext db;
        private readonly IFileEnvironment fileEnv;
        private readonly IFileRepo fileRepo;
        private readonly ILogger<FileController> logger;

        public FileController(IFileService fileService, CaseContext _db, IFileEnvironment fileEnv, IFileRepo fileRepo, ILogger<FileController> logger)
        {
            this.fileService = fileService;
            db = _db;
            this.fileEnv = fileEnv;
            this.fileRepo = fileRepo;
            this.logger = logger;
        }



        [HttpPost("upload")]
        public async Task<IActionResult> uploadFiles(IFormFile file)
        {
            try
            {
                var ext = Path.GetExtension(file.FileName);
                var url = $"{Guid.NewGuid()}{ext}";
                var contentType = file.ContentType;
                using Stream stream = file.OpenReadStream();
                bool result = await fileService.UploadFileAsync(stream, url);
                if (result)
                {
                    var caseFile = new CaseFile()
                    {
                        FileURL = url,
                        ContentType = contentType,
                        Extension = ext,
                        FileSize = stream.Length,
                        FileName = file.FileName
                    };
                    var created = await fileRepo.Insert(caseFile);
                    if (created)
                    {
                        //var val = this.Request.Host;
                        //caseFile.URL = $@"{(Request.IsHttps?@"https://":@"http://")}{Request.Host.Value}/api/file/download/{caseFile.Id}";


                        return Ok(MapURL(caseFile));

                    }
                    else
                    {
                        await fileService.DeleteFileAsync(url);
                    }
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {

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

                bool result = await fileService.FileExistsAsync(casefile.FileURL);

                if (!result)
                {
                    casefile = MapURL(casefile);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return Ok(casefile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("process")]
        public async Task<IActionResult> ProcessCaseFiles()
        {
            try
            {
                var newCaseFiles = db.ScheduledCaseFiles.Include(s => s.CaseFile)
                .ThenInclude(s => s.Case).AsAsyncEnumerable();
                await foreach (var item in newCaseFiles)
                {
                    if (await fileService.FileExistsAsync(item.CaseFile.FileURL))
                    {
                        var newPath = $@"{item.CaseFile.Case.CaseUrl}/{item.CaseFile.FileURL}";
                        await fileService.MoveFileAsync(item.CaseFile.FileURL, newPath);
                        item.CaseFile.FileURL = newPath;
                        await db.SaveChangesAsync();
                    }

                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }



        //private string MapURL(int caseId)
        //{
        //    return $@"{(Request.IsHttps ? @"https://" : @"http://")}{Request.Host.Value}/api/file/download/{caseId}";

        //}
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