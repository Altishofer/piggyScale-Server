using PiggyScaleApi.Models;


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PiggyScaleApi.DTOs;
using PiggyScaleApi.Repositories;

namespace PiggyScaleApi.Services;

using System;
using System.Threading.Tasks;
using PiggyScaleApi.Models;

public class WeightService
{
    private readonly ApplicationContext _context;
    private readonly WeightRepository _weightRepository;
    private readonly IConfiguration _configuration;
    private readonly String _tokenSecret;
    private static readonly TimeSpan tokenLifetime = TimeSpan.FromHours(24);

    public WeightService(ApplicationContext context, IConfiguration configuration)
    {
        _context = context;
        _weightRepository = new WeightRepository(_context);
        _configuration = configuration;
        _tokenSecret = _configuration["JWT_SETTINGS_KEY"];
    }

    public async Task<bool> WeightExistsByWeightName(string weightName)
    {
        return await _weightRepository.WeightExistsByWeightName(weightName);
    }
    
    public async Task<Weight> CreateWeight(WeightDto weightDto)
    {
        Weight weight = weightDto.toEntity();
        weight.weightId = await _weightRepository.NextWeightId();
        return await _weightRepository.SaveWeight(weight);
    }
    
    public async Task<Weight?> GetWeightOrNull(WeightDto weightDto)
    {
        return await _weightRepository.GetWeightByWeightName(weightDto.weightName);
    }
    
    public async Task<List<Weight>> GetAllWeights()
    {
        return await _weightRepository.GetAllWeights();
    }

    public async Task<Weight?> VerifyWeight(ClaimsPrincipal context)
    {
        string? weightnameClaim;
        string? passwordClaim;
        string? idClaim;
        try
        {
            weightnameClaim = context.Claims.FirstOrDefault(c => c.Type == "weightName").Value;
            passwordClaim = context.Claims.FirstOrDefault(c => c.Type == "weightPassword").Value;
            //idClaim = context.Claims.FirstOrDefault(c => c.Type == "weightId").Value;
        } catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        WeightDto claimDto = new WeightDto(weightnameClaim, passwordClaim);

        Weight? weight = await GetWeightOrNull(claimDto);

        return weight ?? null ;
    }

    public async Task<string> GenerateToken(Weight weight)
    {
        return await GenerateToken(new WeightDto(weight.weightName, weight.weightPassword));
    }
    
    public void Delete(Weight weight)
    {
        _weightRepository.DeleteWeight(weight);
    }
    
    public async Task<string> GenerateToken(WeightDto registerWeightDto)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(_tokenSecret);

        List<Claim> claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, registerWeightDto.weightName),
            new("weightName", registerWeightDto.weightName),
            new("weightPassword", registerWeightDto.weightPassword)
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