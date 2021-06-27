using DAL.Models;
using GP_API.Repos;
using GP_API.Services;
using GP_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CaseController : ControllerBase
    {
        private readonly ICaseRepo caseRepo;
        private readonly IFileRepo fileRepo;
        private readonly IFileService fileService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICaseFileUrlMapper fileUrlMapper;

        public CaseController(ICaseRepo _db,IFileRepo fileRepo, IFileService _fileService,UserManager<ApplicationUser> _userManager,ICaseFileUrlMapper _fileUrlMapper)
        {
            this.caseRepo = _db;
            this.fileRepo = fileRepo;
            this.fileService = _fileService;
            userManager = _userManager;
            fileUrlMapper = _fileUrlMapper;


            //fileUrlMapper.ActionRouteString = Url.Action("Download", nameof(FileController));
            //fileUrlMapper.TemplateString = "FileURL-d61b8182e027-FileURL";

        }

        /*Create a Case */
        [HttpPost]
        public async Task<IActionResult> Post(Case _case)
        {
            
            try
            {
                if (_case == null)
                {
                    return BadRequest(new { message = "Data is missing" });
                }


                //add user to case
                //var user = await userManager.GetUserAsync(this.User);
                //_case.User = user;

                //ClaimsPrincipal currentUser = this.User;
                //var currentUserName = currentUser.FindFirst(ClaimTypes.Email).Value;
                //ApplicationUser user = await userManager.FindByNameAsync(currentUserName);
                //_case.User = user;

                var email = this.User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
                if(email != null)
                {
                    var user = await userManager.FindByEmailAsync(email);
                    _case.User = user;
                }

                var created = await caseRepo.Insert(_case);

                return Created("", new { @case = _case, created = created });
            }
            catch (Exception ex)
            {
                //logging
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "internal server error" });
            }
        }

        /*Get Case*/
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {

                var _case = await caseRepo.Get(id);

                if(_case == null)
                    return NotFound(new { message = "Case Not Found" });

                MapURLs(_case.CaseFiles);
                return Ok(_case);

            }

            catch (Exception ex)
            {
                //logging here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "internal server error" });
            }
        }


        /*Get All Cases*/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cases = await caseRepo.GetAll();

                //if (cases != null && cases.Any())
                //    return Ok(new { cases = cases });
                if(cases == null)
                    return NotFound(new { message = "No Cases is found" });

                foreach (var item in cases)
                {
                    MapURLs(item.CaseFiles);
                }
                return Ok(new { cases = cases });
            }
            catch (Exception ex)
            {
                //logging here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
            }
        }


        /*get Cases with page*/
        [HttpGet("cases")]
        public async Task<IActionResult> GetAll(int page)
        {
            try
            {
                var cases = await caseRepo.GetAll(page);

                //if (cases != null && cases.Any())
                //    return Ok(new { cases = cases });
                if (cases == null)
                    return NotFound(new { message = "No Cases is found" });

                foreach (var item in cases)
                {
                    MapURLs(item.CaseFiles);
                }
                return Ok(new { cases = cases });

            }
            catch (Exception ex)
            {

                //logging here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
            }
        }


        /*Search Cases*/
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchModel searchModel)
        {
            try
            {
                var list = new List<int>();
                var cases = await caseRepo.Search(searchModel);
                //if (cases != null && cases.Any())
                //    return Ok(new { cases = cases });
                if (cases == null)
                    return NotFound(new { message = "No Cases is found" });

                foreach (var item in cases)
                {
                    MapURLs(item.CaseFiles);
                }
                return Ok(cases);
            }
            catch (Exception ex)
            {

                //logging here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
            }
        }

        /*Update Case*/
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Case _case)
        {
            try
            {
                if (id != _case.Id)
                {
                    return BadRequest(new { message = "IDs don't match" });
                }

                var updated = await caseRepo.Update(id, _case);
                if (updated)
                    return Accepted(new { updated = updated });

                return NotFound(new { message = "Case Not Found" });

            }
            catch (Exception ex)
            {

                //logging here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
            }
        }

        /*Delete Case*/
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {

                var mycase = await caseRepo.Get(id);
                if (mycase == null)
                    return NotFound(new { message = "Case Not Found" });

                var deleted = await caseRepo.Delete(id);

                mycase.CaseFiles?.ToList()
                    .ForEach(async c =>
                    {
                        await fileService.DeleteFileAsync(c.FileURL);
                    });

                return Ok();

            }
            catch (Exception ex)
            {

                //logging here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
            }
        }



        private void MapURL(CaseFile file)
        {
            file.URL = @$"{GetDownloadActionUrl()}/{file.Id}";
        }

        private void MapURLs(IEnumerable<CaseFile> files)
        {
            foreach (var file in files)
            {
                MapURL(file);
            }
        }

        private string GetDownloadActionUrl()
        {
            //return $@"{(Request.IsHttps ? @"https://" : @"http://")}{Request.Host.Value}/api/file/download";
            return $@"{fileUrlMapper.DownloadActionUrl}";
        }

    }


}
