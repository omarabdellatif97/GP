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

        public FileController(IFileService fileService,IFileEnvironment fileEnv)
        {
            this.fileService = fileService;
            this.fileEnv = fileEnv;
        }

        [HttpGet]
        public IActionResult GetFile()
        {
            return Ok("hello world");
        }



        //// /api/files
        //public async IActionResult PostAsync()
        //{
        //    IFormCollection form = await Request.ReadFormAsync();
        //    // wirte server
        //    List<string> urls = new List<string>();

        //    IFormFile file = form.Files.First();
        //    CaseFile casefiel = new CaseFile();
        //    casefiel.FileName = file.FileName;


        //    //form.Files.First().OpenReadStream().Length
        //    //return urls of files 
        //    return Ok(urls);
        //}

        [HttpPost]
        public async Task<IActionResult> PostAsync(IFormFile file)
        {
            IFormCollection form = await Request.ReadFormAsync();
            // wirte server
            List<string> urls = new List<string>();

            string exptension = Path.GetExtension(file.FileName);
            // .pdf or .sql
            // 
            //ftp://192.169.2.3/app/lablab/
            //ftp://192.169.2.3/app/lablab/root/filename;
            // relative server path /app/lablab/root/filename
            //IRemoteFile remote = path.NewRemoteFile(Guid.NewGuid().ToString());

            //IRemoteResourceInfo remote = path.NewRemotePath();
            fileService.UploadFile(file.OpenReadStream(),"");

            //ftp://192.169.2.3/root/lablab/app/filename;
            
            //// for file service
            //remote.RelativeRootFileName;// root == /root/lablab/app/filename

            //// for database
            //remote.RelativeContentFileName; // app == /app/filename

            CaseFile casefiel = new CaseFile();
            //casefiel.FileURL = remote.RelativePath;
            casefiel.FileName = file.FileName;


            //form.Files.First().OpenReadStream().Length
            //return urls of files 
            return null;
        }

        //// /api/files
        //public async List<string> PostAsync()
        //{
        //    IFormCollection form = await Request.ReadFormAsync();
        //    // wirte server
        //    List<string> urls = new List<string>();

        //    IFormFile file = form.Files.First();
        //    CaseFile casefiel = new CaseFile();
        //    casefiel.FileName = file.FileName;


        //    //form.Files.First().OpenReadStream().Length
        //    //return urls of files 
        //    return Ok(urls);
        //}

        //// app/klajsdfjajsdfjasldflasdf.pdf
        //public IActionResult Get(string url)
        //{
        //    byte[] data = this.fileService.DownloadFile(url);

        //    return File(data,"");

        //}






    }
}


// casefiles delete Action
// casefile post
// casefiles post
// 

// advanced
// ------------------------------------------------------------
// limit file size before action and controller creation.