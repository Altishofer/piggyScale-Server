
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PiggyScaleApi.Models;
using Host = PiggyScaleApi.Models.User;

namespace Project.DTOs;

public class ReturnUserDto
{
    [Required]
    public long userId { get; set; }
    [Required]
    public string userName { get; set; }
    
    [JsonConstructor]
    public ReturnUserDto(){}

    public ReturnUserDto(User host)
    {
        userId = host.userId;
        userName = host.userName;
    }
}