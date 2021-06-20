using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppsController : ControllerBase
    {
        private readonly CaseContext context;

        public AppsController(CaseContext context)
        {
            this.context = context;
        }

        // GET: api/<AppsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> Get()
        {
            return await context.Applications.ToListAsync();
        }
    }
}
