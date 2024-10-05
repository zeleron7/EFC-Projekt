using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DbRepos;
using Models;
using Services;
using Configuration;

namespace AppWebApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class csAdminController : Controller
    {
        IAttractionService _iAttractionService = null;

        //Seed database
        [HttpGet()]
        [ActionName("Seed")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> SeedDatabaseAsync()
        {
            try
            {
                await _iAttractionService.SeedDatabaseAsync();
                return Ok("Sedeed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        public csAdminController(IAttractionService service)
        {
            _iAttractionService = service;
        }
    
    }
}



