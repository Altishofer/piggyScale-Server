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
        private readonly UserService _userService;

        public WeightController(ApplicationContext context, IConfiguration config)
        {
            _userService = new UserService(context, config);
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

        [Authorize]
        [HttpPost("store")]
        public async Task<IActionResult> Store([FromBody] PostWeightDto postWeightDto)
        {
            User? claimUser = await _userService.VerifyUser(HttpContext.User);
            if (claimUser == null)
            {
                return Unauthorized("The token could not be validated");
            }
            
            await _weightService.Save(postWeightDto, claimUser.userId);
            return Ok();
        }

        [Authorize]
        [HttpPost("deleteLast/{userId}")]
        public async Task<IActionResult> Store()
        {
            User? claimUser = await _userService.VerifyUser(HttpContext.User);
            if (claimUser == null)
            {
                return Unauthorized("The token could not be validated");
            }
            
            Weight weight = await _weightService.DeleteLastByUserId(claimUser.userId);
            return Ok(weight.ToDto());
        }

        [Authorize]
        [HttpGet("box/{boxId}/{days}")]
        public async Task<IActionResult> Store([FromRoute] uint boxId, [FromRoute] uint days)
        {
            User? claimUser = await _userService.VerifyUser(HttpContext.User);
            if (claimUser == null)
            {
                return Unauthorized("The token could not be validated");
            }
            
            return Ok(await _weightService.GetWeightsByBoxNumberAndDays(boxId, days, claimUser.userId));
        }

        [AllowAnonymous]
        [HttpGet("write")]
        public async Task<IActionResult> WriteTestData()
        {
            await _weightService.GenerateTestData();
            return Ok();
        }

        [Authorize]
        [HttpGet("export")]
        public async Task<IActionResult> ExportAll()
        {
            User? claimUser = await _userService.VerifyUser(HttpContext.User);
            if (claimUser == null)
            {
                return Unauthorized("The token could not be validated");
            }
            
            return Ok(await _weightService.ExportAllByUserId(claimUser.userId));
        }
    }
}