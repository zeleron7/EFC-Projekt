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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class csAdminController : Controller
    {
        IAttractionRepo _iAttractionService = null;
        
        //Seed database
        [HttpGet()]
        [ActionName("Seed")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> SeedDatabaseAsync()
        {
            try
            {
                _iAttractionService.SeedDatabaseAsync();
                return Ok("Sedeed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        public csAdminController(IAttractionRepo service)
        {
            _iAttractionService = service;
        }
    }
}



