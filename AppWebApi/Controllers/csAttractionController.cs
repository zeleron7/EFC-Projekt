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

        //Clear database
        [HttpDelete("Clear")]
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

        //GET: api/Attractions/Read
        [HttpGet()]
        [ActionName("ReadAttractions")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadAttractions(string seeded = "true", string flat = "true",
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
    

        //GET: api/Attractions/ReadWithoutComments
        [HttpGet()]
        [ActionName("ReadWithoutComment")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadWithoutComments(string seeded = "true", string flat = "true",
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
     
                var _resp = await _service.ReadAttractionsWithoutCommentsAsync(_seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);     
                return Ok(_resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //GET: api/Attractions/ReadOneAttraction
        [HttpGet()]
        [ActionName("ReadOneAttraction")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadOneAttraction(Guid id)
        {
            _logger.LogInformation("Endpoint Attractions executed");

            var attraction = await _service.ReadOneAttractionAsync(id);
            if (attraction == null)
            {
                return NotFound(); // Returns 404 response
            }
 
            return Ok(attraction);

        }

        //GET: api/Users/Read
        [HttpGet()]
        [ActionName("ReadUsers")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IUser>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadUsers(string seeded = "true", string flat = "true",
            string filter = null, string pageNr = "0", string pageSize = "10")
        {
            _logger.LogInformation("Endpoint Users executed");
             bool _seeded = bool.Parse(seeded);
             bool _flat = bool.Parse(flat);
            int _pageNr = int.Parse(pageNr);
            int _pageSize = int.Parse(pageSize);

            var users = await _service.ReadUsers(_seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);
            if(users == null)
            {
                return NotFound(); // Returns 404 response
            }

            return Ok(users);  // Return the data with 200 OK status

        }

        public csAttractionController(IAttractionService service, ILogger<csAttractionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        
    }
}