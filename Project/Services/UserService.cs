using PiggyScaleApi.Models;


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PiggyScaleApi.DTOs;
using PiggyScaleApi.DTOs.UserDto;
using PiggyScaleApi.Repositories;

namespace PiggyScaleApi.Services;

using System;
using System.Threading.Tasks;
using PiggyScaleApi.Models;

public class UserService
{
    private readonly ApplicationContext _context;
    private readonly UserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly String _tokenSecret;
    private static readonly TimeSpan tokenLifetime = TimeSpan.FromHours(24);

    public UserService(ApplicationContext context, IConfiguration configuration)
    {
        _context = context;
        _userRepository = new UserRepository(_context);
        _configuration = configuration;
        _tokenSecret = _configuration["JWT_SETTINGS_KEY"];
    }

    public async Task<bool> UserExistsByUserName(string userName)
    {
        return await _userRepository.UserExistsByUserName(userName);
    }
    
    public async Task<User> CreateUser(RegisterUserDto userDto)
    {
        User user = userDto.toEntity();
        user.userId = await _userRepository.NextUserId();
        return await _userRepository.SaveUser(user);
    }
    
    public async Task<User?> GetUserOrNull(RegisterUserDto userDto)
    {
        return await _userRepository.GetUserByUserName(userDto.userName);
    }
    
    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }

    public async Task<User?> VerifyUser(ClaimsPrincipal context)
    {
        string? usernameClaim;
        string? passwordClaim;
        string? idClaim;
        try
        {
            usernameClaim = context.Claims.FirstOrDefault(c => c.Type == "userName").Value;
            passwordClaim = context.Claims.FirstOrDefault(c => c.Type == "userPassword").Value;
            //idClaim = context.Claims.FirstOrDefault(c => c.Type == "userId").Value;
        } catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        RegisterUserDto claimDto = new RegisterUserDto(usernameClaim, passwordClaim);

        User? user = await GetUserOrNull(claimDto);

        return user ?? null ;
    }

    public async Task<string> GenerateToken(User user)
    {
        return await GenerateToken(new RegisterUserDto(user.userName, user.userPassword));
    }
    
    public void Delete(User user)
    {
        _userRepository.DeleteUser(user);
    }
    
    public async Task<string> GenerateToken(RegisterUserDto registerUserDto)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(_tokenSecret);

        List<Claim> claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, registerUserDto.userName),
            new("userName", registerUserDto.userName),
            new("userPassword", registerUserDto.userPassword)
        };

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(tokenLifetime),
            Issuer = _configuration["JWT_SETTINGS_ISSUER"],
            Audience = _configuration["JWT_SETTINGS_AUDIENCE"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        SecurityToken token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

        String jwt = jwtSecurityTokenHandler.WriteToken(token);
            
        return jwt;
    }
}