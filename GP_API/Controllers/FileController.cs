using GP_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        //// /api/files
        //public async List<string> PostAsync()
        //{
        //    IFormCollection form = await Request.ReadFormAsync();
        //    // wirte server
        //    List<string> urls = new List<string>();
            
            

        //    //return urls of files 
        //    return Ok(urls);
        //}
    }
}
