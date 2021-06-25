using DAL.Models;
using GP_API.Repos;
using GP_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CaseController : ControllerBase
    {
        private readonly ICaseRepo db;
        private readonly IFileService fileService;
        private readonly UserManager<ApplicationUser> userManager;

        public CaseController(ICaseRepo _db, IFileService _fileService,UserManager<ApplicationUser> _userManager)
        {
            this.db = _db;
            this.fileService = _fileService;
            userManager = _userManager;
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

                var created = await db.Insert(_case);

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

                var _case = await db.Get(id);

                if (_case != null)
                    return Ok(_case);

                return NotFound(new { message = "Case Not Found" });
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
                var cases = await db.GetAll();

                if (cases != null && cases.Any())
                    return Ok(new { cases = cases });

                return NotFound(new { message = "No Cases is found" });
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
                var cases = await db.GetAll(page);

                if (cases != null & cases.Any())
                    return Ok(new { @case = cases });

                return NotFound(new { message = "Case Not Found" });

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
                var cases = await db.Search(searchModel);
                if (cases != null)
                    return Ok(cases);

                return NotFound(new { message = "Case Not Found" });
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

                var updated = await db.Update(id, _case);

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

                var mycase = await db.Get(id);
                if (mycase == null)
                    return NotFound(new { message = "Case Not Found" });

                var deleted = await db.Delete(id);

                mycase.CaseFiles?.ToList()
                    .ForEach(async c => await fileService.DeleteFileAsync(c.FileURL));

                return Ok();

            }
            catch (Exception ex)
            {

                //logging here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
            }
        }




    }
}
