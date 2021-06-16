using DAL.Models;
using GP_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class FileController : ControllerBase
    {
        private readonly IFileService fileService;
        private readonly IFileEnvironment fileEnv;
        private readonly IFileRepo fileRepo;
        private readonly ILogger<FileController> logger;

        public FileController(IFileService fileService,IFileEnvironment fileEnv, IFileRepo fileRepo, ILogger<FileController> logger)
        {
            this.fileService = fileService;
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

                bool result = await fileService.UploadFileAsync(file.OpenReadStream(), url);
                if (result)
                {
                    var caseFile = new CaseFile()
                    {
                        FileURL = url,
                        ContentType = contentType,
                        Extension = ext,
                        FileName = file.FileName
                    };
                    var created = await fileRepo.Insert(caseFile);
                    if (created)
                        //return Ok(new { url });
                        return Ok(caseFile);
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
        public async Task<IActionResult> downloadFile(string id)
        {
            try
            {
                var casefile = await fileRepo.GetById(id);
                if(casefile == null)
                    return NotFound(new { message = $"File not found with ID = {id}" });
                
                var file = await fileService.DownloadFileAsync(casefile.FileURL);
                return Ok(File(file, $"application/{casefile.ContentType}"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> downloadFileWithUrl(string url)
        {
            try
            {
                var casefile = await fileRepo.Get(url);
                if (casefile == null)
                    return NotFound(new { message = $"File not found with ID = {url}" });

                var file = await fileService.DownloadFileAsync(casefile.FileURL);
                return Ok(File(file, $"application/{casefile.ContentType}"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("delete")]
        public async Task<IActionResult> deleteFileWithUrl(string url)
        {
            try
            {
                var casefile = await fileRepo.Get(url);
                if (casefile == null)
                    return NotFound(new { message = $"File not found with ID = {url}" });

                await fileRepo.Delete(casefile.Id);


                return Ok(casefile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("exists")]
        public async Task<IActionResult> IsFileExists(string url)
        {
            try
            {
                var casefile = await fileRepo.Get(url);
                if (casefile == null)
                    return NotFound(new { message = $"File not found with ID = {url}" });

                bool result = await fileService.FileExistsAsync(casefile.FileURL);
                
                if(!result)
                {
                    
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return Ok(casefile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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