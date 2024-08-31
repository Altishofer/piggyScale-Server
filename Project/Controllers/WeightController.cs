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
        [HttpPost("store")]
        public async Task<IActionResult> Store([FromBody] PostWeightDto postWeightDto)
        {
            await _weightService.Save(postWeightDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("deleteLast/{userId}")]
        public async Task<IActionResult> Store([FromRoute] uint userId)
        {
            Weight weight = await _weightService.DeleteLastByUserId(userId);
            return Ok(weight.ToDto());
        }

        [AllowAnonymous]
        [HttpGet("box/{boxId}/{days}/{userId}")]
        public async Task<IActionResult> Store([FromRoute] uint boxId, [FromRoute] uint days, [FromRoute] long userId)
        {
            return Ok(await _weightService.GetWeightsByBoxNumberAndDays(boxId, days, userId));
        }

        [AllowAnonymous]
        [HttpGet("write")]
        public async Task<IActionResult> WriteTestData()
        {
            await _weightService.GenerateTestData();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("export/{userId}")]
        public async Task<IActionResult> ExportAll(long userId)
        {
            return Ok(await _weightService.ExportAllByUserId(userId));
        }
    }
}