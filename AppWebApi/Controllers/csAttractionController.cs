using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Seido.Utilities.SeedGenerator;

using Models;
using Services;
using Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class csAttractionController : Controller
    {
        private ILogger<csAttractionController> _logger = null;
        private IAttractionService _service = null;

        //GET: api/csAdmin/Attractions
        [HttpGet()]
        [ActionName("Attractions")]
        [ProducesResponseType(200, Type = typeof(List<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Attractions(string count = "5")
        {
            _logger.LogInformation("Endpoint Attractions executed");
           try
            {
                _logger.LogInformation("Endpoint Attractions executed");
                int _count = int.Parse(count);

                //List<IAttraction> attractions = await _service.Attractions(_count);
                var attractions = new csAttraction();
                return Ok(attractions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        public csAttractionController(IAttractionService service, ILogger<csAttractionController> logger)
        {
            _service = service;
            _logger = logger;
        }
    }
}