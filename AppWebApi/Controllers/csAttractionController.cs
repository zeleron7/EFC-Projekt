using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
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


        /*//GET: api/csAdmin/Attractions
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
           
        }*/


    

        // Endpoint to delete data from database
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearDatabase()
        {
            try
            {
                await _service.ClearDatabaseAsync();
                return Ok("Database cleared successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //GET: api/addresses/read
        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(string seeded = "true", string flat = "true",
            string filter = null, string pageNr = "0", string pageSize = "10")
        {
            _logger.LogInformation("Endpoint Attractions executed");
            try
            {
                 _logger.LogInformation("Endpoint Attractions executed");
                bool _seeded = bool.Parse(seeded);
                bool _flat = bool.Parse(flat);
                int _pageNr = int.Parse(pageNr);
                int _pageSize = int.Parse(pageSize);
     
                var _resp = await _service.ReadAttractionsAsync(_seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);     
                return Ok(_resp);
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