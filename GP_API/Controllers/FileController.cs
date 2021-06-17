using DAL.Models;
using GP_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public FileController(IFileService fileService,IFileEnvironment fileEnv, IFileRepo fileRepo)
        {
            this.fileService = fileService;
            this.fileEnv = fileEnv;
            this.fileRepo = fileRepo;
        }

    

        [HttpPost("upload")]
        public async Task<IActionResult> uploadFiles(IFormFile file)
        {
            try
            {
                HttpContext.Connection.LocalPort;
                var ext = Path.GetExtension(file.FileName);
                var url = $"{Guid.NewGuid()}.{ext}";
                var contentType = file.ContentType;

                bool result = fileService.UploadFile(file.OpenReadStream(), url);
                if (result)
                {
                    var created = await fileRepo.Insert(new CaseFile() {
                        FileURL = url, 
                        ContentType = contentType,
                        Extension = ext,
                        FileName = file.FileName });
                    if (created)
                        return Ok(new { url });
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
                
                var file = fileService.DownloadFile(Path.GetFileName(casefile.FileURL));
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

                var file = fileService.DownloadFile(Path.GetFileName(casefile.FileURL));
                return Ok(File(file, $"application/{casefile.ContentType}"));
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