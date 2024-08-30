using Project;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyScaleApi.DTOs.UserDto;
using PiggyScaleApi.Models;
using PiggyScaleApi.Services;

namespace PiggyScaleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightController : ControllerBase
    {
        private readonly WeightService _WeightService;
        private readonly ApplicationContext _context;
        private readonly IConfiguration _config;

        public WeightController(ApplicationContext applicationContext, IConfiguration config)
        {
            _context = applicationContext;
            _config = config;
            _WeightService = new WeightService(_context, _config);
        }
        
        [AllowAnonymous]
        [HttpGet("status")]
        public async Task<ActionResult>  GetStatus()
        {
            return Ok(new
            {
                message = "works"
            });
        }
        
        [AllowAnonymous]
        [HttpGet("validate/{word}")]
        public async Task<IActionResult> CheckValidity([FromRoute] string word)
        {
            return Ok("hello");
        }
        
        [AllowAnonymous]
        [HttpPut("wordCalculation")]
        public async Task<IActionResult> GetWordCalcDebug([FromBody] RegisterUserDto dto)
        {
            return Ok("hello");
        }
        
    }
}