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
    // public class csWapiInfo
    // {
    //     public string Environment { get; set; }
    //     public string DbConnection { get; set; }
    //     public DbSetDetail DbSetActive { get; set; }
    // }
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class csAdminController : Controller
    {
        // private ILogger<csAdminController> _logger = null;

        IAttractionRepo _iAttractionService = null;
        
        [HttpGet()]
        [ActionName("TestSeed")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> RobustSeedAsync()
        {
            try
            {
                _iAttractionService.RobustSeedAsync();
                return Ok("Sedeed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

       

        //GET: api/csAdmin/Info
        // [HttpGet()]
        // [ActionName("Info")]
        // [ProducesResponseType(200, Type = typeof(csWapiInfo))]
        // [ProducesResponseType(400, Type = typeof(string))]
        // public async Task<IActionResult> Info()
        // {
        //     try
        //     {
        //         _logger.LogInformation("Endpoint Info executed");
        //         var _info = new csWapiInfo {
        //             Environment = csAppConfig.ASPNETCOREEnvironment,
        //             DbSetActive = csAppConfig.DbSetActive
        //         };
                
        //         return Ok(_info);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex.Message);
        //         return BadRequest(ex.Message);
        //     }           
        // }

        //GET: api/csAdmin/AfricanAnimals
        // [HttpGet()]
        // [ActionName("AfricanAnimals")]
        // [ProducesResponseType(200, Type = typeof(List<IAnimal>))]
        // [ProducesResponseType(400, Type = typeof(string))]
        // public async Task<IActionResult> AfricanAnimals(string count = "10")
        // {
        //     try
        //     {
        //         _logger.LogInformation("Endpoint AfricanAnimals executed");
        //         int _count = int.Parse(count);

        //         List<IAnimal> animals = await _service.AfricanAnimals(_count);
        //         return Ok(animals);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex.Message);
        //         return BadRequest(ex.Message);
        //     }
           
        // }

        // GET: api/csAdmin/AfricanAnimals
        // [HttpGet()]
        // [ActionName("Seed")]
        // [ProducesResponseType(200, Type = typeof(string))]
        // [ProducesResponseType(400, Type = typeof(string))]
        // public async Task<IActionResult> Seed(string count = "10")
        // {
        //     try
        //     {
        //         _logger.LogInformation("Endpoint Seed executed");
        //         int _count = int.Parse(count);


        //         await _service.Seed(_count);
        //         return Ok("Seeded");
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex.Message);
        //         return BadRequest(ex.Message);
        //     }           
        // }

        // GET: api/csAdmin/log
        // [HttpGet()]
        // [ActionName("Log")]
        // [ProducesResponseType(200, Type = typeof(IEnumerable<csLogMessage>))]
        // public async Task<IActionResult> Log([FromServices] ILoggerProvider _loggerProvider)
        // {
        //     //Note the way to get the LoggerProvider, not the logger from Services via DI
        //     if (_loggerProvider is csInMemoryLoggerProvider cl)
        //     {
        //         return Ok(await cl.MessagesAsync);
        //     }
        //     return Ok("No messages in log");
        // }
        public csAdminController(IAttractionRepo service)
        {
            _iAttractionService = service;
            
            // _logger = logger;
        }
    }
}



