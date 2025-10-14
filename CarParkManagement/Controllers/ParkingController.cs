using CarParkManagement.DataAccess.Data;
using CarParkManagement.DataAccess.Data.Models;
using CarParkManagement.DataAccess.Data.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CarParkManagement.DataAccess.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Collections.Immutable;
using CarParkManagement.DataAccess.Data.Services;
using CarParkManagement.DataAccess.Data.Services.IServices;

namespace CarParkManagement.Controllers
{
    [ApiController]
    [Route("parking")]
    public class ParkingController : ControllerBase
    {

        //private readonly ILogger<ParkingController> _logger;
        private readonly IParkingService _parkingService;

        public ParkingController(IParkingService parkingService)
        {
            //_logger = logger;
            _parkingService = parkingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableSpace()
        {
            try
            {
                var result = await _parkingService.GetAvailableSpace(); 
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult>  OccupySpace([FromBody] OccupySpaceDto request)
        {
            
            try
            {
               var response = await _parkingService.OccupySpace(request);
                return Ok(response);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }            

        }

        [HttpPost("exit")]
        public async Task<IActionResult> ToexitAway([FromBody] ParkingVehicleRegDto request)
        {
            try
            {
               var response = await _parkingService.ToexitAway(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            
        }

    }
}
