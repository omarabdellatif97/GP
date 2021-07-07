using DAL.Models;
using GP_API.Repos;
using GP_API.Services;
using GP_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    //[Authorize]
    public class CaseController : ControllerBase
    {
        private readonly ILogger<CaseController> logger;
        private readonly ICaseRepo caseRepo;
        private readonly ICaseFileRepo fileRepo;
        private readonly IFileService fileService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICaseFileUrlMapper fileUrlMapper;

        public CaseController(ILogger<CaseController> logger, ICaseRepo _db, ICaseFileRepo fileRepo, IFileService _fileService, UserManager<ApplicationUser> _userManager, ICaseFileUrlMapper _fileUrlMapper)
        {
            this.logger = logger;
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

                var email = this.User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
                if (email != null)
                {
                    var user = await userManager.FindByEmailAsync(email);
                    _case.User = user;
                }

                var casefilesIds = _case.CaseFiles.Select(c => c.Id).ToList();
                var descriptionIds = this.fileUrlMapper.ExtractIds(_case.Description);
                
                _case.Description = this.fileUrlMapper.GenerateTemplate(_case.Description);

                // remove any id that exists in the case file ids
                descriptionIds.RemoveAll(id => casefilesIds.Any(c => c == id));

                var allIds = casefilesIds.Union(descriptionIds).Distinct().ToList();

                if (allIds.Any())
                {

                    var caseFiles = await fileRepo.GetAll(allIds);


                    _case.CaseUrl = $@"Cases/Case-{Guid.NewGuid()}";
                    await fileService.CreateDirectoryAsync(_case.CaseUrl);
                    _case.CaseFiles.Clear();


                    foreach (var file in caseFiles)
                    {

                        var newPath = $@"{_case.CaseUrl}/{file.FileURL}";
                        await fileService.MoveFileAsync(file.FileURL, newPath);
                        file.FileURL = newPath;

                        if (descriptionIds.Any(i => i == file.Id))
                            file.IsDescriptionFile = true;

                        _case.CaseFiles.Add(file);
                    }
                }

                var result = await caseRepo.InsertAsync(_case);

                return Created("", new { @case = _case, created = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "internal server error" });
            }
        }

        /*Get Case*/
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {

                var _case = await caseRepo.GetAsync(id);

                if (_case == null)
                    return NotFound(new { message = "Case Not Found" });

                _case.Description = fileUrlMapper.GenerateDescription(_case.Description);
                MapURLs(_case.CaseFiles);
                return Ok(_case);

            }

            catch (Exception ex)
            {

                logger.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "internal server error" });
            }
        }


        /*Get All Cases*/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cases = await caseRepo.GetAllAsync();

                //if (cases != null && cases.Any())
                //    return Ok(new { cases = cases });
                if (cases == null)
                    return NotFound(new { message = "No Cases is found" });

                foreach (var item in cases)
                {
                    item.Description = fileUrlMapper.GenerateDescription(item.Description);
                    MapURLs(item.CaseFiles);
                }

                return Ok(new { cases = cases });
            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
            }
        }


        /*get Cases with page*/
        [HttpGet("cases")]
        public async Task<IActionResult> GetAll(int page)
        {
            try
            {
                var cases = await caseRepo.GetAllAsync(page);

                //if (cases != null && cases.Any())
                //    return Ok(new { cases = cases });
                if (cases == null)
                    return NotFound(new { message = "No Cases is found" });

                foreach (var item in cases)
                {
                    item.Description = fileUrlMapper.GenerateDescription(item.Description);
                    MapURLs(item.CaseFiles);
                }
                return Ok(new { cases = cases });

            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message, ex);
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
                var cases = await caseRepo.SearchAsync(searchModel);
                //if (cases != null && cases.Any())
                //    return Ok(new { cases = cases });
                if (cases == null)
                    return NotFound(new { message = "No Cases is found" });

                foreach (var item in cases)
                {
                    item.Description = fileUrlMapper.GenerateDescription(item.Description);
                    MapURLs(item.CaseFiles);
                }
                return Ok(cases);
            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
            }
        }

        /*Update Case*/
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Case _case)
        {
            try
            {
                if (_case == null || id != _case.Id)
                {
                    return BadRequest(new { message = "IDs don't match" });
                }

                var casefilesIds = _case.CaseFiles.Select(c => c.Id ).ToList();
                var descriptionIds = this.fileUrlMapper.ExtractIds(_case.Description);

                _case.Description = this.fileUrlMapper.GenerateTemplate(_case.Description);

                // remove any id that exists in the case file ids
                descriptionIds.RemoveAll(id => casefilesIds.Any(c => c == id));

                var allIds = casefilesIds.Union(descriptionIds).Distinct().ToList();

                if (allIds.Any())
                {

                    var caseFiles = await fileRepo.GetAll(allIds);

                    if(_case.CaseUrl == null)
                    {
                        _case.CaseUrl = $@"Cases/Case-{Guid.NewGuid()}";
                        await fileService.CreateDirectoryAsync(_case.CaseUrl);
                    }

                    _case.CaseFiles.Clear();


                    foreach (var file in caseFiles)
                    {
                        if(file.CaseId == null)
                        {
                            var newPath = $@"{_case.CaseUrl}/{file.FileURL}";
                            await fileService.MoveFileAsync(file.FileURL, newPath);
                            file.FileURL = newPath;
                        }

                        // if case file not exist in the ids of case files, it is a description file
                        if (!casefilesIds.Any(i=> i== file.Id))
                            file.IsDescriptionFile = true;


                        _case.CaseFiles.Add(file);
                    }

                }

                var result = await caseRepo.UpdateAsync(id, _case);
                if (result)
                    return Accepted(new { updated = result});
                else
                    throw new Exception("update failed");
            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
            }
        }

        /*Delete Case*/
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {

                var mycase = await caseRepo.GetAsync(id);
                if (mycase == null)
                    return NotFound(new { message = "Case Not Found" });

                var deleted = await caseRepo.DeleteAsync(id);

                //mycase.CaseFiles?.ToList()
                //    .ForEach(async c =>
                //    {
                //        await fileService.DeleteFileAsync(c.FileURL);
                //    });


                await fileService.DeleteDirectoryAsync(mycase.CaseUrl);

                return Ok();

            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message, ex);
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
