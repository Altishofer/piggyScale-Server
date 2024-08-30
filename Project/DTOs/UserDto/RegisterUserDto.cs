using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using User = PiggyScaleApi.Models.User;

namespace PiggyScaleApi.DTOs.UserDto;

public class RegisterUserDto
{
    [Required]
    public string userName { get; set; }
    [Required]
    public string userPassword { get; set; }
    
    [JsonConstructor]
    public RegisterUserDto(){}

    public RegisterUserDto(User user)
    {
        userName = user.userName;
        userPassword = user.userPassword;
    }
    
    public RegisterUserDto(string userName, string userPassword)
    {
        this.userName = userName;
        this.userPassword = userPassword;
    }
    
    public User toEntity()
    {
        User user = new User();
        user.userPassword = userPassword;
        user.userName = userName;
        
        return user;
    }
}