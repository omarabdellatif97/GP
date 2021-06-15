using DAL.Models;
using GP_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly ICaseRepo db;

        public CaseController( ICaseRepo _db )
        {
            this.db = _db;
        }

        /*Create a Case */
        [HttpPost]
        public async Task<IActionResult> Post(Case _case)
        {
            if(_case == null){
                return BadRequest(new { errors = "Data is missing" });
            }
            //TODO: calling create from repository
            //var created = await db.Insert(_case);

            return Created("",/*new { @case=_case, created=created }*/new { @case=_case});
        }

        /*Get Case*/
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            /*var _case = await db.Get(id);

            if (_case != null)
                return Ok(new { @case = _case });
*/
            return NotFound(new { message = "Case Not Found" });
        }


        /*Get All Cases*/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            /*var cases =  await db.GetAll().ToList();

            if (cases != null && cases.Count != 0)
                return Ok(new { cases = cases });
*/
            return NotFound(new { message = "No Cases is found" });
        }


        /*Search Cases*/
        //[HttpGet]
        //public async Task<IActionResult> Search(int id)
        //{
        //    var _case = await db.Get(id);

        //    if (_case != null)
        //        return Ok(new { @case = _case });

        //    return NotFound(new { message = "Case Not Found" });
        //}


        /*Search Cases*/
        [HttpGet]
        public async Task<IActionResult> Search(string title, string [] tags)
        {
            var _cases = db.Search(title, tags);

            if (_cases != null)
                return Ok(_cases);

            return NotFound(new { message = "Case Not Found" });
        }

        /*Update Case*/
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Case _case)
        { 
            if(id != _case.Id)
            {
                return BadRequest(new { message = "IDs don't match" });
            }

            /*var updated = await db.Update(id, _case);

            if(updated)
                return Accepted(new { updated = updated });*/

            return NotFound(new { message = "Case Not Found"});
            
        }

        /*Delete Case*/
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            /*var deleted = await db.Delete(id);

            if(deleted)
                return Accepted(new { deleted = deleted });*/

            return NotFound(new { message = "Case Not Found" });

        }




    }
}
