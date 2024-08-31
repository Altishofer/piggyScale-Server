using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PiggyScaleApi.DTOs.UserDto;
using PiggyScaleApi.Models;
using PiggyScaleApi.Services;
using User = PiggyScaleApi.Models.User;

namespace PiggyScaleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(ApplicationContext context, IConfiguration config)
        {
            _userService = new UserService(context, config);
        }
        
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The format of the credentials are not valid");
            }
            if (await _userService.UserExistsByUserName(userDto.userName))
            {
                return BadRequest("Username is already taken, please choose another one");
            }

            User user = await _userService.CreateUser(userDto);
            return CreatedAtAction(nameof(Register), new {token = _userService.GenerateToken(userDto), id = user.userId }); 
        }
        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The format of the credentials were not valid");
            }

            User? authUser = await _userService.GetUserOrNull(userDto);
            if (authUser == null || authUser.userPassword != userDto.userPassword)
            {
                return Unauthorized("User with given credentials was not found");
            }

            return Ok(new {token = _userService.GenerateToken(userDto), id = authUser.userId});
        }
        
        [HttpGet("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            User? claimUser = await _userService.VerifyUser(HttpContext.User);
            if (claimUser == null)
            {
                return Unauthorized("The token could not be validated");
            }

            return Ok(_userService.GenerateToken(claimUser));
        }
    }
}