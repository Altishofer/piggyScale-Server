using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyScaleApi.DTOs;
using PiggyScaleApi.Models;
using PiggyScaleApi.Services;

namespace PiggyScaleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightController : ControllerBase
    {
        private readonly WeightService _weightService;

        public WeightController(ApplicationContext context)
        {
            _weightService = new WeightService(context);
        }
        
        [AllowAnonymous]
        [HttpGet("status")]
        public async Task<ActionResult> GetStatus()
        {
            return Ok(new
            {
                message = "works"
            });
        }
        
        [AllowAnonymous]
        [HttpGet("store")]
        public async Task<IActionResult> Store([FromBody] WeightDto weightDto)
        {
            await _weightService.Save(weightDto);
            return Ok();
        }
        
    }
}