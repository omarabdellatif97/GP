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

        public CaseController( ICaseRepo db )
        {
            this.db = db;
        }


        public async Task<IActionResult> PostAsync(Case mycase)
        {
            
            return Ok();
        }

    }
}
